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

    public async Task InitDatabase()
    {
        if (!_unitOfWork.LevelTypeRepository.GetAll().Any())
        {
            await _unitOfWork.BeginTransactionAsync();
            var gameMode = new List<LevelType>()
            {
                new LevelType
                {
                    Id = 1,
                    TypeName = "Basic"
                },
                new LevelType
                {
                    Id = 2,
                    TypeName = "Sequence"
                },
                new LevelType
                {
                    Id = 3,
                    TypeName = "Loop"
                },
                new LevelType
                {
                    Id = 4,
                    TypeName = "Function"
                },
                new LevelType
                {
                    Id = 5,
                    TypeName = "Condition"
                },
                new LevelType
                {
                    Id = 6,
                    TypeName = "Custom"
                }
            };

            var positionTypes = new List<PositionType>()
            {
                new PositionType
                {
                    Id = 1,
                    TypeName = "Board"
                },
                new PositionType
                {
                    Id = 2,

                    TypeName = "Target"
                },
                new PositionType
                {
                    Id = 3,
                    TypeName = "Rock"
                },
            };

            try
            {
                await _unitOfWork.LevelTypeRepository.ForceAddRangeAsync(gameMode);
                await _unitOfWork.PositionTypeRepository.ForceAddRangeAsync(positionTypes);
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            await _unitOfWork.CommitAsync();
        }

        var baseData = _unitOfWork.GameLevelRepository.GetAll();

        // Add Basic 
        if (!baseData.Any(o => o.GameLevelTypeId == 1))
        {
            var sampleBasicLevel = new List<ModifiedLevelDataRequest>()
            {
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    VStartPosition = 26,
                    LevelIndex = 0,
                    GameLevelTypeId = 1,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 27,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 28,
                            TypeId = 2
                        },
                    }
                },
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    LevelIndex = 1,
                    VStartPosition = 26,
                    GameLevelTypeId = 1,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 27,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 28,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 29,
                            TypeId = 2
                        },
                    }
                },
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    LevelIndex = 2,
                    VStartPosition = 26,
                    GameLevelTypeId = 1,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 27,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 28,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 29,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 21,
                            TypeId = 2
                        },
                    }
                }
            };

            foreach (var basic in sampleBasicLevel)
            {
                await AddNewLevel(basic);
            }
        }

        // Add Sequence
        if (!baseData.Any(o => o.GameLevelTypeId == 2))
        {
            var sampleSequenceLevel = new List<ModifiedLevelDataRequest>()
            {
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    VStartPosition = 26,
                    GameLevelTypeId = 2,
                    LevelIndex = 0,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 29,
                            TypeId = 2
                        },
                    }
                },
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    VStartPosition = 26,
                    LevelIndex = 1,
                    GameLevelTypeId = 2,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 21,
                            TypeId = 2
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 29,
                            TypeId = 2
                        },
                    }
                },
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    VStartPosition = 9,
                    LevelIndex = 2,
                    GameLevelTypeId = 2,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 28,
                            TypeId = 2
                        },
                    }
                }
            };

            foreach (var sequence in sampleSequenceLevel)
            {
                await AddNewLevel(sequence);
            }
        }

        // LOOP GAME
        if (!baseData.Any(o => o.GameLevelTypeId == 3))
        {
            var sampleLoopLevels = new List<ModifiedLevelDataRequest>()
            {
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    LevelIndex = 0,
                    VStartPosition = 26,
                    GameLevelTypeId = 3,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 27,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 29,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 28,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 30,
                            TypeId = 2
                        },
                    }
                },
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    LevelIndex = 1,
                    VStartPosition = 25,
                    GameLevelTypeId = 3,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 26,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 27,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 28,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 29,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 30,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 31,
                            TypeId = 2
                        },
                    }
                },
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    LevelIndex = 2,
                    VStartPosition = 43,
                    GameLevelTypeId = 3,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 35,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 27,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 19,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 11,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 12,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 13,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 14,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 15,
                            TypeId = 2
                        },
                    }
                },
            };

            foreach (var basic in sampleLoopLevels)
            {
                await AddNewLevel(basic);
            }
        }

        // FUNC GAME
        if (!baseData.Any(o => o.GameLevelTypeId == 4))
        {
            var sampleFuncLevels = new List<ModifiedLevelDataRequest>()
            {
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    LevelIndex = 0,
                    VStartPosition = 26,
                    GameLevelTypeId = 4,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 27,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 28,
                            TypeId = 2
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 29,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 30,
                            TypeId = 2
                        },
                    }
                },
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    LevelIndex = 1,
                    VStartPosition = 35,
                    GameLevelTypeId = 4,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 27,
                            TypeId = 2
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 28,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 20,
                            TypeId = 2
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 21,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 13,
                            TypeId = 2
                        },
                    }
                },
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    LevelIndex = 2,
                    VStartPosition = 25,
                    GameLevelTypeId = 4,
                    LevelDetail = new List<LevelDetailRequest>()
                    {
                        new LevelDetailRequest
                        {
                            VPosition = 26,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 27,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 28,
                            TypeId = 2
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 29,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 30,
                            TypeId = 2
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 31,
                            TypeId = 1
                        },
                        new LevelDetailRequest
                        {
                            VPosition = 32,
                            TypeId = 2
                        },
                    }
                },
            };

            foreach (var func in sampleFuncLevels)
            {
                await AddNewLevel(func);
            }
        }
    }

    #region GAME CLIENT

    public async Task<List<ModeType>> GetAllGameMode()
    {
        var result = await _unitOfWork.GameLevelRepository
            .GetAll()
            .GroupBy(h => h.GameLevelType)
            .Select(group => new ModeType
            {
                IdMode = group.Key.Id,
                TypeName = group.Key.TypeName ?? "Null Name",
                totalLevel = group.Count()
            })
            .ToListAsync();

        var allMode = _unitOfWork.LevelTypeRepository.GetAll();

        foreach (var mode in allMode)
        {
            if (result.Any(o => o.IdMode == mode.Id))
            {
                continue;
            }
            else
            {
                result.Add(new ModeType()
                {
                    IdMode = mode.Id,
                    TypeName = mode.TypeName ?? "Null Name",
                    totalLevel = 0
                });
            }
        }

        if (result.Count == 0)
        {
            var result3 = await _unitOfWork.LevelTypeRepository.GetAll().ToListAsync();
            foreach (var item in result3)
            {
                result.Add(new ModeType
                {
                    IdMode = item.Id,
                    TypeName = item.TypeName ?? "Null Name",
                    totalLevel = 0
                });
            }
        }

        return result;
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

        var result = new LevelInformationResponse
        {
            CoinReward = firstItem.CoinReward ?? 0,
            GameReward = firstItem.GemReward ?? 0,
            VStartPosition = firstItem.VStartPosition,
            levelDetail = levelInformation.Select(item => new LevelPositionData
            {
                VPosition = item.VPosition,
                PositionType = item.PositionTypeId
            }).ToList()
        };

        return result;
    }

    public async Task<UserDataResponse> UserFinishLevel(UserFinishLevelRequest userFinishLevelRequest)
    {
        var winLevel =
            await GetGameLevelByTypeAndIndex(userFinishLevelRequest.ModeId, userFinishLevelRequest.LevelIndex);
        if (winLevel == null)
        {
            throw new BadRequestException("Level not found");
        }

        var oldData = await _unitOfWork.GamePlayHistoryRepository.GetAsync(
            o => o.StudentId == userFinishLevelRequest.UserID
                 && o.GameLevelTypeId == userFinishLevelRequest.ModeId
                 && o.LevelIndex == userFinishLevelRequest.LevelIndex
            , null
        ).ContinueWith(o => o.Result.FirstOrDefault());

        var userData = await _unitOfWork.GameUserProfileRepository.GetAsync(
            o => o.StudentId == userFinishLevelRequest.UserID, null).ContinueWith(o => o.Result.FirstOrDefault());
        var result = new UserDataResponse
        {
            UserId = userData.StudentId,
            DisplayName = userData.DisplayName,
            OldGem = (int)userData.Gem,
            OldCoin = (int)userData.Coin,
            UserCoin = userData.Gem,
            UserGem = userData.Coin
        };
        // COIN ADD
        await _unitOfWork.BeginTransactionAsync();
        if (oldData == null) // first play -> add  coin
        {
            userData.Coin += winLevel.CoinReward ?? 0;
            userData.Gem += winLevel.GemReward ?? 0;

            result.UserGem = userData.Gem;
            result.UserCoin = userData.Coin;
            _unitOfWork.GameUserProfileRepository.Update(userData);
        }

        await _unitOfWork.GamePlayHistoryRepository.AddAsync(new GamePlayHistory
        {
            LevelIndex = userFinishLevelRequest.LevelIndex,
            PlayTime = userFinishLevelRequest.StartTime,

            FinishTime = DateTime.Now,
            GameLevelTypeId = userFinishLevelRequest.ModeId,
            Duration = (DateTime.Now - userFinishLevelRequest.StartTime).Minutes,
            StudentId = userFinishLevelRequest.UserID
        });

        try
        {
            await _unitOfWork.SaveChangeAsync();
            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        return result;
    }

    #endregion

    #region ADMIN SERVICES

    public async Task AddNewLevel(ModifiedLevelDataRequest modifiedLevelData)
    {
        await _unitOfWork.BeginTransactionAsync();

        var query =
            await _unitOfWork.GameLevelRepository.GetAsync(o
                    => o.GameLevelTypeId == modifiedLevelData.GameLevelTypeId && o.LevelIndex != -1
                , null);
        var currentLevels = query.ToList();
        var currentMaxLevel = currentLevels.Max(o => o.LevelIndex) ?? -1;

        if (modifiedLevelData.LevelIndex < 0)
        {
            throw new BadRequestException("LevelIndex not valid");
        }

        if (modifiedLevelData.LevelIndex - currentMaxLevel > 1) // add at last
        {
            modifiedLevelData.LevelIndex = currentMaxLevel + 1;
        }
        else if (modifiedLevelData.LevelIndex <= currentMaxLevel) // replace current level
        {
            try
            {
                foreach (var currentLevel in currentLevels)
                {
                    if (currentLevel.LevelIndex >= modifiedLevelData.LevelIndex)
                    {
                        currentLevel.LevelIndex++;
                    }

                    _unitOfWork.GameLevelRepository.Update(currentLevel);
                }
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        var gameLevelId = 0;
        var newData = new GameLevel
        {
            LevelIndex = modifiedLevelData.LevelIndex,
            CoinReward = modifiedLevelData.CoinReward,
            GemReward = modifiedLevelData.GemReward,
            Max = 0,
            VStartPosition = modifiedLevelData.VStartPosition,
            GameLevelTypeId = modifiedLevelData.GameLevelTypeId,
        };

        // ADD GAME LEVEL
        try
        {
            await _unitOfWork.GameLevelRepository.AddAsync(newData);
            await _unitOfWork.SaveChangeAsync();
            var result = await GetGameLevelByTypeAndIndex(newData.GameLevelTypeId, (int)newData.LevelIndex);
            gameLevelId = result!.Id;
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();

            throw;
        }

        // ADD DETAILS
        var details = new List<GameLevelDetail>();
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
            await _unitOfWork.SaveChangeAsync();

            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();

            throw;
        }
    }

    public async Task UpdateLevel(ModifiedLevelDataRequest modifiedLevelData)
    {
        // check Existed

        await _unitOfWork.BeginTransactionAsync();
        var query =
            await _unitOfWork.GameLevelRepository.GetAsync(o
                    => o.GameLevelTypeId == modifiedLevelData.GameLevelTypeId
                , null).ContinueWith(o => o.Result.ToList());

        var checkExisted =
            query.FirstOrDefault(o => o.Id == modifiedLevelData.Id);
        var currentLevels = query.Where(o => o.LevelIndex != -1).ToList();
        var currentMaxLevel = currentLevels.Max(o => o.LevelIndex) ?? -1;
        if (checkExisted == null)
        {
            throw new BadRequestException("Game level not found");
        }

        if (modifiedLevelData.LevelIndex < 0)
        {
            throw new BadRequestException("LevelIndex not valid");
        }

        // Move updated level out
        if (checkExisted.LevelIndex != modifiedLevelData.LevelIndex)
        {
            if (modifiedLevelData.LevelIndex < checkExisted.LevelIndex)
            {
                try
                {
                    foreach (var itemLevel in currentLevels)
                    {
                        if (itemLevel.LevelIndex >= modifiedLevelData.LevelIndex
                            && itemLevel.LevelIndex < checkExisted.LevelIndex
                           )
                        {
                            itemLevel.LevelIndex++;
                            _unitOfWork.GameLevelRepository.Update(itemLevel);
                        }
                    }
                }
                catch (Exception e)
                {
                    await _unitOfWork.RollbackAsync();
                    throw;
                }
            }

            if (modifiedLevelData.LevelIndex > checkExisted.LevelIndex)
            {
                try
                {
                    foreach (var itemLevel in currentLevels)
                    {
                        if (itemLevel.LevelIndex > checkExisted.LevelIndex
                            && itemLevel.LevelIndex <= modifiedLevelData.LevelIndex
                           )
                        {
                            itemLevel.LevelIndex--;
                            _unitOfWork.GameLevelRepository.Update(itemLevel);
                        }
                    }
                }
                catch (Exception e)
                {
                    await _unitOfWork.RollbackAsync();
                    throw;
                }
            }
        }

        if (modifiedLevelData.LevelIndex > currentMaxLevel) // update to last index
        {
            modifiedLevelData.LevelIndex = currentMaxLevel;
        }

        // REMOVE CURRENT DETAILS

        try
        {
            var currentList = await
                _unitOfWork.GameLevelDetailRepository
                    .GetAsync(o => o.GameLevelId == modifiedLevelData.Id, null);
            _unitOfWork.GameLevelDetailRepository.DeleteRange(
                currentList
            );
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        // ADD NEW DETAILS
        List<GameLevelDetail> details = new List<GameLevelDetail>();
        foreach (var detail in modifiedLevelData.LevelDetail)
        {
            details.Add(new GameLevelDetail
            {
                GameLevelId = modifiedLevelData.Id,
                VPosition = detail.VPosition,
                PositionTypeId = detail.TypeId
            });
        }

        try
        {
            // Update current game level

            checkExisted.Id = checkExisted.Id;
            checkExisted.LevelIndex = modifiedLevelData.LevelIndex;
            checkExisted.CoinReward = modifiedLevelData.CoinReward;
            checkExisted.GemReward = modifiedLevelData.GemReward;
            checkExisted.VStartPosition = modifiedLevelData.VStartPosition;
            checkExisted.GameLevelTypeId = modifiedLevelData.GameLevelTypeId;
            _unitOfWork.GameLevelRepository.Update(checkExisted);
            // Add details
            await _unitOfWork.GameLevelDetailRepository.AddRangeAsync(
                details
            );
            await _unitOfWork.SaveChangeAsync();
            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<LevelDataResponse> GetLevelDataById(int id)
    {
        var query = await _unitOfWork.GameLevelRepository.GetAsync(o => o.Id == id, null);
        var gameLevel = query.FirstOrDefault();
        if (gameLevel == null)
        {
            throw new NotFoundException("Game level not found");
        }

        var levelInformation = await _unitOfWork.GameLevelDetailRepository
            .GetAsync(o => o.GameLevelId == gameLevel.Id, null);

        var result = new LevelDataResponse
        {
            Id = gameLevel.Id,
            LevelIndex = gameLevel.LevelIndex ?? 0,
            CoinReward = gameLevel.CoinReward ?? 0,
            GemReward = gameLevel.GemReward ?? 0,
            VStartPosition = gameLevel.VStartPosition,
            GameLevelTypeId = gameLevel.GameLevelTypeId,
            LevelDetail = levelInformation.Select(item => new LevelDetail()
            {
                VPosition = item.VPosition,
                TypeId = item.PositionTypeId,
            }).ToList()
        };

        return result;
    }

    public async Task<List<LevelDataResponse>> GetLevelsByMode(int modeId)
    {
        var query = await _unitOfWork.GameLevelRepository.GetAsync(
            o => o.GameLevelTypeId == modeId && o.LevelIndex != -1, null
            , includeProperties: $"{nameof(GameLevel.GameLevelType)}");
        if (!query.Any())
        {
            throw new NotFoundException("Not found any level");
        }

        var result = query.Select(gameLevel => new LevelDataResponse()
        {
            Id = gameLevel.Id,
            LevelIndex = gameLevel.LevelIndex ?? 0,
            CoinReward = gameLevel.CoinReward ?? 0,
            GemReward = gameLevel.GemReward ?? 0,
            VStartPosition = gameLevel.VStartPosition,
            GameLevelTypeId = gameLevel.GameLevelTypeId,
            GameLevelTypeName = gameLevel.GameLevelType.TypeName,
            LevelDetail = new List<LevelDetail>()
        }).OrderBy(o => o.LevelIndex).ToList();

        foreach (var resultItem in result)
        {
            var levelInformation = await _unitOfWork.GameLevelDetailRepository
                .GetAsync(o => o.GameLevelId == resultItem.Id, null);

            resultItem.LevelDetail = levelInformation.Select(item => new LevelDetail()
            {
                VPosition = item.VPosition,
                TypeId = item.PositionTypeId,
            }).ToList();
        }

        return result;
    }

    /// <summary>
    /// SWAP INDEX OF TWO LEVEL GAME
    /// </summary>
    /// <param name="modifiedLevelData">request param</param>
    public async Task UpdateLevelIndex(ModifiedLevelIndex modifiedLevelData)
    {
        var query = await _unitOfWork.GameLevelRepository.GetAsync(
            o => o.Id == modifiedLevelData.IdA || o.Id == modifiedLevelData.IdB, null);
        var gameLevel = query.ToList();
        if (gameLevel.Count != 2)
        {
            throw new NotFoundException("Game level not found");
        }

        if (gameLevel[0].GameLevelTypeId != gameLevel[1].GameLevelTypeId)
        {
            throw new BadRequestException("Game mode does not match");
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            // ReSharper disable once SwapViaDeconstruction
            var temp = gameLevel[0].LevelIndex;
            gameLevel[0].LevelIndex = gameLevel[1].LevelIndex;
            gameLevel[1].LevelIndex = temp;

            _unitOfWork.GameLevelRepository.Update(gameLevel[0]);
            _unitOfWork.GameLevelRepository.Update(gameLevel[1]);
            await _unitOfWork.SaveChangeAsync();
            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task SoftDeleteLevelGame(int id)
    {
        await _unitOfWork.BeginTransactionAsync();
        var checkExisted =
            await _unitOfWork.GameLevelRepository.GetAsync(o
                    => o.GameLevelTypeId == id
                , null).ContinueWith(o => o.Result.FirstOrDefault());
        if (checkExisted == null)
        {
            throw new BadRequestException("Game level not found");
        }

        // check Existed
        var currentLevels =
            await _unitOfWork.GameLevelRepository.GetAsync(o
                    => o.GameLevelTypeId == checkExisted.GameLevelTypeId && o.LevelIndex != -1
                , null).ContinueWith(o => o.Result.ToList());
        if (checkExisted == null)
        {
            throw new BadRequestException("Game level not found");
        }

        foreach (var itemLevel in currentLevels)
        {
            if (itemLevel.LevelIndex > checkExisted.LevelIndex)
            {
                itemLevel.LevelIndex--;
                _unitOfWork.GameLevelRepository.Update(itemLevel);
            }
        }

        checkExisted.LevelIndex = -1;
        try
        {
            // Update current game level
            _unitOfWork.GameLevelRepository.Update(checkExisted);
            await _unitOfWork.SaveChangeAsync();
            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    #endregion

    #region Helper

    private async Task<GameLevel?> GetGameLevelByTypeAndIndex(int gameModeId, int levelIndex)
    {
        var result = await _unitOfWork.GameLevelRepository.GetAsync(
            o => o.GameLevelTypeId == gameModeId && o.LevelIndex == levelIndex
            , null);
        return result.FirstOrDefault();
    }

    #endregion
}