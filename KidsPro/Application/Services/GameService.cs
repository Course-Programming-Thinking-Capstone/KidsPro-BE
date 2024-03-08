using Application.Dtos.Response.Game;
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
}