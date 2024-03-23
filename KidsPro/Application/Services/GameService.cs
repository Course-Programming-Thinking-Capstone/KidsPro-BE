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
        // CHECK FOR SAMPLE ACCOUNT
        // if (await _unitOfWork.StudentRepository.GameStudentLoginAsync(SampleEmail)
        //         .ContinueWith(t => t.Result) == null)
        // {
        //     await _unitOfWork.StudentRepository.AddAsync(new Student
        //     {
        //         Account = new Account
        //         {
        //             Email = SampleEmail,
        //             PasswordHash = null,
        //         },
        //         UserName = "Denk",
        //         GameUserProfile = new GameUserProfile
        //         {
        //             DisplayName = "Denkhotieu",
        //             Coin = 0,
        //             Gem = 0,
        //         },
        //     });
        //     await _unitOfWork.SaveChangeAsync();
        // }

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
            await _unitOfWork.BeginTransactionAsync();

            var sampleBasicLevel = new List<ModifiedLevelDataRequest>()
            {
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
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
                            TypeId = 2
                        },
                    }
                },
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
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
            var sampleSequenceLevel = new List<ModifiedLevelDataRequest>()
            {
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    VStartPosition = 26,
                    GameLevelTypeId = 2,
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
            try
            {
                foreach (var basic in sampleBasicLevel)
                {
                    await AddNewLevel(basic, false);
                }

                foreach (var sequence in sampleSequenceLevel)
                {
                    await AddNewLevel(sequence, false);
                }
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            await _unitOfWork.CommitAsync();
        }

        // Add Sequence
        if (!baseData.Any(o => o.GameLevelTypeId == 2))
        {
            await _unitOfWork.BeginTransactionAsync();

            var sampleSequenceLevel = new List<ModifiedLevelDataRequest>()
            {
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
                    VStartPosition = 26,
                    GameLevelTypeId = 2,
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
            try
            {
                foreach (var sequence in sampleSequenceLevel)
                {
                    await AddNewLevel(sequence, false);
                }
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            await _unitOfWork.CommitAsync();
        }

        // LOOP GAME
        if (!baseData.Any(o => o.GameLevelTypeId == 3))
        {
            await _unitOfWork.BeginTransactionAsync();

            var sampleLoopLevels = new List<ModifiedLevelDataRequest>()
            {
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
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

            try
            {
                foreach (var basic in sampleLoopLevels)
                {
                    await AddNewLevel(basic, false);
                }
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            await _unitOfWork.CommitAsync();
        }

        // FUNC GAME
        if (!baseData.Any(o => o.GameLevelTypeId == 4))
        {
            await _unitOfWork.BeginTransactionAsync();

            var sampleFuncLevels = new List<ModifiedLevelDataRequest>()
            {
                new ModifiedLevelDataRequest
                {
                    CoinReward = 100,
                    GemReward = 100,
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

            try
            {
                foreach (var func in sampleFuncLevels)
                {
                    await AddNewLevel(func, false);
                }
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            await _unitOfWork.CommitAsync();
        }
    }

    #region GAME CLIENT

    public async Task<List<ModeType>> GetAllGameMode()
    {
        var result2 = await _unitOfWork.GameLevelRepository
            .GetAll()
            .GroupBy(h => h.GameLevelType)
            .Select(group => new ModeType
            {
                IdMode = group.Key.Id,
                TypeName = group.Key.TypeName ?? "Null Name",
                totalLevel = group.Count()
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

    #endregion

    #region ADMIN SERVICES

    public async Task AddNewLevel(ModifiedLevelDataRequest modifiedLevelData, bool onTransaction = true)
    {
        var currentList =
            await _unitOfWork.GameLevelRepository.GetAsync(o => o.GameLevelTypeId == modifiedLevelData.GameLevelTypeId,
                null);
        var levelIndex = currentList.Max(o => o.LevelIndex) ?? -1;
        levelIndex++;
        var gameLevelId = 0;
        var newData = new GameLevel
        {
            LevelIndex = levelIndex,
            CoinReward = modifiedLevelData.CoinReward,
            GemReward = modifiedLevelData.GemReward,
            Max = 0,
            VStartPosition = modifiedLevelData.VStartPosition,
            GameLevelTypeId = modifiedLevelData.GameLevelTypeId
        };
        var details = new List<GameLevelDetail>();
        if (onTransaction)
        {
            await _unitOfWork.BeginTransactionAsync();
        }

        try
        {
            await _unitOfWork.GameLevelRepository.AddAsync(newData);
            await _unitOfWork.SaveChangeAsync();
            var result = await GetGameLevelByTypeAndIndex(newData.GameLevelTypeId, (int)newData.LevelIndex);
            gameLevelId = result!.Id;
        }
        catch (Exception e)
        {
            if (onTransaction)
            {
                await _unitOfWork.RollbackAsync();
            }

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
            await _unitOfWork.SaveChangeAsync();
            if (onTransaction)
            {
                await _unitOfWork.CommitAsync();
            }
        }
        catch (Exception e)
        {
            if (onTransaction)
            {
                await _unitOfWork.RollbackAsync();
            }

            throw;
        }
    }

    public async Task UpdateLevel(ModifiedLevelDataRequest modifiedLevelData)
    {
        // check Existed
        var checkExisted =
            await GetLevelDataById(modifiedLevelData.Id);
        if (checkExisted == null)
        {
            throw new BadRequestException("Game level not found");
        }

        // REMOVE CURRENT DETAILS
        await _unitOfWork.BeginTransactionAsync();
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
            _unitOfWork.GameLevelRepository.Update(new GameLevel
            {
                Id = checkExisted.Id,
                LevelIndex = checkExisted.LevelIndex,
                CoinReward = modifiedLevelData.CoinReward,
                GemReward = modifiedLevelData.GemReward,
                VStartPosition = modifiedLevelData.VStartPosition,
                GameLevelTypeId = modifiedLevelData.GameLevelTypeId,
            });
            await _unitOfWork.GameLevelDetailRepository.AddRangeAsync(
                details
            );
            await _unitOfWork.SaveChangeAsync();
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
        var query = await _unitOfWork.GameLevelRepository.GetAsync(o => o.GameLevelTypeId == modeId, null);
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