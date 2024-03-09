using Application.Dtos.Request.Game;
using Application.Dtos.Response.Game;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class GameService : IGameService
{
    private IUnitOfWork _unitOfWork;
    private ILogger<AccountService> _logger;

    public GameService(IUnitOfWork unitOfWork, ILogger<AccountService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<ModeType>> GetAllGameMode()
    {
        var result2 = await _unitOfWork.GameLevelRepository
            .GetAll()
            .GroupBy(h => h.GameLevelType)
            .Select(group => new ModeType
            {
                IdMode = group.Key.Id,
                TypeName = group.Key.TypeName ?? "Null Name",
                totalLevel = group.Max(h => h.LevelIndex) ?? 0
            })
            .ToListAsync();

        if (result2.Count == 0)
        {
            var result3 = await _unitOfWork.LevelTypeRepository.GetAll().ToListAsync();
            foreach (var item in result3)
            {
                result2.Add(new ModeType
                {
                    IdMode = item.Id,
                    TypeName = item.TypeName ?? "Null Name",
                    totalLevel = 0
                });
            }
        }

        return result2;
    }

    public async Task<List<CurrentLevelData>> GetUserCurrentLevel(int userId)
    {
        var result = await _unitOfWork.GamePlayHistoryRepository
            .GetAll()
            .Where(h => h.StudentId == userId) // Filter by userId
            .GroupBy(h => h.GameLevelType)
            .Select(group => new CurrentLevelData
            {
                Mode = group.Key.Id,
                LevelIndex = group.Max(h => h.LevelIndex)
            })
            .ToListAsync();

        return result;
    }

    public async Task<LevelInformationResponse?> GetLevelInformation(int typeId, int levelIndex)
    {
        var gameLevel = await _unitOfWork.GameLevelRepository
            .GetAsync(o => o.GameLevelTypeId == typeId && o.LevelIndex == levelIndex, null);

        var firstItem = gameLevel.FirstOrDefault();

        if (firstItem == null)
            return null;

        var levelInformation = await _unitOfWork.GameLevelDetailRepository
            .GetAsync(o => o.GameLevelId == firstItem.Id, null);

        return new LevelInformationResponse
        {
            CoinReward = firstItem.CoinReward ?? 0,
            GameReward = firstItem.GameReward ?? 0,
            VStartPosition = firstItem.VStartPosition,
            levelDetail = levelInformation.Select(item => new LevelPositionData
            {
                VPosition = item.VPosition,
                TypeName = item.PositionType.TypeName
            }).ToList()
        };
    }

    public async Task<int> UserFinishLevel(UserFinishLevelRequest userFinishLevelRequest)
    {
        var winLevel =
            await GetGameLevelByTypeAndIndex(userFinishLevelRequest.ModeId, userFinishLevelRequest.LevelIndex);
        if (winLevel == null)
        {
            throw new BadRequestException("Level not found");
        }

        var isPlayedHistory = await _unitOfWork.GamePlayHistoryRepository.GetAsync(
            o => o.StudentId == userFinishLevelRequest.UserID
                 && o.GameLevelTypeId == userFinishLevelRequest.ModeId
                 && o.LevelIndex == userFinishLevelRequest.LevelIndex
            , null
        );
        var userCoin = -1;
        var oldData = isPlayedHistory.FirstOrDefault();
        if (oldData == null) // already play -> no coin
        {
            var userData = await _unitOfWork.GameUserProfileRepository.GetByIdAsync(userFinishLevelRequest.UserID);
            var coinWin = winLevel.CoinReward ?? 0;
            userData.Coin += coinWin;
            userCoin = userData.Coin;
            _unitOfWork.GameUserProfileRepository.Update(userData);
        }

        await _unitOfWork.GamePlayHistoryRepository.AddAsync(new GamePlayHistory
        {
            LevelIndex = userFinishLevelRequest.LevelIndex,
            PlayTime = userFinishLevelRequest.StartTime,
            FinishTime = userFinishLevelRequest.EndTime,
            GameLevelTypeId = userFinishLevelRequest.ModeId,
            Duration = (userFinishLevelRequest.EndTime - userFinishLevelRequest.StartTime).Minutes,
            StudentId = userFinishLevelRequest.UserID
        });

        return userCoin;
    }

    public async Task AddNewLevel(ModifiedLevelDataRequest modifiedLevelData)
    {
        var checkExisted =
            await GetGameLevelByTypeAndIndex(modifiedLevelData.GameLevelTypeId, modifiedLevelData.LevelIndex);
        if (checkExisted != null)
        {
            throw new BadRequestException("This level with game mode " + modifiedLevelData.GameLevelTypeId +
                                          " at index " + modifiedLevelData.LevelIndex + " already existed");
        }

        var gameLevelId = 0;
        var newData = new GameLevel
        {
            LevelIndex = modifiedLevelData.LevelIndex,
            CoinReward = modifiedLevelData.CoinReward,
            GameReward = modifiedLevelData.GemReward,
            Max = 0,
            VStartPosition = modifiedLevelData.VStartPosition,
            GameLevelTypeId = modifiedLevelData.GameLevelTypeId
        };
        var details = new List<GameLevelDetail>();
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _unitOfWork.GameLevelRepository.AddAsync(newData);
            var result = await GetGameLevelByTypeAndIndex(newData.GameLevelTypeId, (int)newData.LevelIndex);
            gameLevelId = result!.Id;
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        foreach (var detail in modifiedLevelData.LevelDetail)
        {
            details.Add(new GameLevelDetail
            {
                VPosition = detail.VPosition,
                GameLevelId = gameLevelId,
                PositionTypeId = detail.TypeId
            });
        }

        try
        {
            await _unitOfWork.GameLevelDetailRepository.AddRangeAsync(
                details
            );
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task<GameLevel?> GetGameLevelByTypeAndIndex(int gameModeId, int levelIndex)
    {
        var result = await _unitOfWork.GameLevelRepository.GetAsync(
            o => o.GameLevelTypeId == gameModeId && o.LevelIndex == levelIndex
            , null);
        return result.FirstOrDefault();
    }
}