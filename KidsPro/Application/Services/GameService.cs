using Application.Dtos.Request.Game;
using Application.Dtos.Response.Game;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Entities;
using Domain.Enums;
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
                new()
                {
                    Id = 1,
                    TypeName = "Basic"
                },
                new()
                {
                    Id = 2,
                    TypeName = "Sequence"
                },
                new()
                {
                    Id = 3,
                    TypeName = "Loop"
                },
                new()
                {
                    Id = 4,
                    TypeName = "Function"
                },
                new()
                {
                    Id = 5,
                    TypeName = "Condition"
                },
                new()
                {
                    Id = 6,
                    TypeName = "Custom"
                }
            };

            var positionTypes = new List<PositionType>()
            {
                new()
                {
                    Id = 1,
                    TypeName = "Board"
                },
                new()
                {
                    Id = 2,

                    TypeName = "Target"
                },
                new()
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
            catch (Exception)
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

            foreach (var func in sampleFuncLevels)
            {
                await AddNewLevel(func);
            }
        }

        // Game ShopItem
        var baseItem = _unitOfWork.GameItemRepository.GetAll();
        if (!baseItem.Any(o => o.ItemType == ItemType.ShopItem))
        {
            var tempData = new List<NewItemRequest>()
            {
                new NewItemRequest
                {
                    ItemName = "Allue",
                    Details =
                        "Allue is mischievous and curious, always fond of exploring and delighting others with its adorable personality and playful antics.",
                    SpritesUrl = "Allue",
                    ItemRateType = 0,
                    ItemType = 0,
                    Price = 100
                },
                new NewItemRequest
                {
                    ItemName = "Auesitle",
                    Details =
                        "Auesitle is an adventurous being, constantly seeking new discoveries with a playful curiosity that knows no bounds",
                    SpritesUrl = "Auesitle",
                    ItemRateType = 1,
                    ItemType = 0,
                    Price = 500
                },
                new NewItemRequest
                {
                    ItemName = "Wirder",
                    Details =
                        "Wirder is a mysterious individual, known for their eccentricities and unconventional lifestyle. They possess a unique perspective, constantly questioning norms and exploring the depths of the unknown, captivating others with their enigmatic presence.",
                    SpritesUrl = "Wirder",
                    ItemRateType = 1,
                    ItemType = 0,
                    Price = 500
                },
                new NewItemRequest
                {
                    ItemName = "Baqua",
                    Details =
                        "Baqua is a resilient warrior, embodying strength and determination in every step. With a heart full of courage, Baqua faces challenges head-on, inspiring others with their unwavering resolve and indomitable spirit.",
                    SpritesUrl = "Baqua",
                    ItemRateType = 1,
                    ItemType = 0,
                    Price = 500
                },
                new NewItemRequest
                {
                    ItemName = "Wene",
                    Details =
                        "Wene is a spirited individual, radiating warmth and kindness wherever she goes. She possesses a resilient spirit, navigating life's challenges with grace and determination, inspiring those around her with her unwavering optimism.",
                    SpritesUrl = "Wene",
                    ItemRateType = 3,
                    ItemType = 0,
                    Price = 1000
                },
                new NewItemRequest
                {
                    ItemName = "Beir",
                    Details =
                        "She is a resilient warrior, wielding her blade with precision and grace. With unwavering determination, Beir faces every challenge head-on, inspiring those around her with her courage and strength.",
                    SpritesUrl = "Beir",
                    ItemRateType = 3,
                    ItemType = 0,
                    Price = 1000
                },
                new NewItemRequest
                {
                    ItemName = "Spotine",
                    Details =
                        "Spotine is a spirited individual, radiating warmth and positivity wherever she goes. With a contagious energy, she uplifts those around her, bringing joy and laughter to every encounter.",
                    SpritesUrl = "Spotine",
                    ItemRateType = 3,
                    ItemType = 0,
                    Price = 1000
                },
                new NewItemRequest
                {
                    ItemName = "Crimson",
                    Details =
                        "Crimson is a fierce warrior, renowned for his unmatched skill and unwavering determination. He commands respect with his imposing presence, leading with courage and valor on the battlefield.",
                    SpritesUrl = "Crimson",
                    ItemRateType = 3,
                    ItemType = 0,
                    Price = 1500
                },
                new NewItemRequest
                {
                    ItemName = "Risir",
                    Details =
                        "Risir is a cunning strategist, wielding intellect and wit to overcome any challenge. He navigates complexities with ease, his sharp mind always one step ahead of his adversaries.",
                    SpritesUrl = "Risir",
                    ItemRateType = 3,
                    ItemType = 0,
                    Price = 1500
                },
                new NewItemRequest
                {
                    ItemName = "Dupomo",
                    Details =
                        "Dupomo is a gentle soul, exuding kindness and compassion in every action. She brings harmony wherever she goes, her soothing presence a source of comfort to those around her.",
                    SpritesUrl = "Dupomo",
                    ItemRateType = 3,
                    ItemType = 0,
                    Price = 1500
                },
                new NewItemRequest
                {
                    ItemName = "Pomo",
                    Details =
                        "Pomo is a lively and energetic character, always ready for adventure and excitement. He approaches life with enthusiasm and optimism, spreading joy and laughter wherever he goes.",
                    SpritesUrl = "Pomo",
                    ItemRateType = 5,
                    ItemType = 0,
                    Price = 2000
                },
                new NewItemRequest
                {
                    ItemName = "GeDusk",
                    Details =
                        "GeDusk is a mysterious figure, cloaked in shadows and secrets. She moves with stealth and grace, her presence haunting yet captivating, leaving others intrigued by her enigmatic aura.",
                    SpritesUrl = "GeDusk",
                    ItemRateType = 5,
                    ItemType = 0,
                    Price = 2000
                },
                new NewItemRequest
                {
                    ItemName = "Glek",
                    Details =
                        "Glek is a stoic and resilient individual, weathered by life's challenges yet unyielding in spirit. He carries himself with quiet strength, his determination evident in every step he takes, inspiring others with his unwavering resolve.",
                    SpritesUrl = "Glek",
                    ItemRateType = 5,
                    ItemType = 0,
                    Price = 2000
                },
                new NewItemRequest
                {
                    ItemName = "Obube",
                    Details =
                        "Obube is a charismatic and dynamic character, radiating charm and charisma in every interaction. She possesses a magnetic personality, effortlessly drawing others to her with her wit and warmth, leaving a lasting impression wherever she goes.",
                    SpritesUrl = "Obube",
                    ItemRateType = 5,
                    ItemType = 0,
                    Price = 3000
                },
                new NewItemRequest
                {
                    ItemName = "Dupomo",
                    Details =
                        "Dupomo is a gentle soul, exuding kindness and compassion in every action. She brings harmony wherever she goes, her soothing presence a source of comfort to those around her.",
                    SpritesUrl = "Dupomo",
                    ItemRateType = 6,
                    ItemType = 0,
                    Price = 3000
                },
                new NewItemRequest
                {
                    ItemName = "Hirgh",
                    Details =
                        "Hirgh is a stoic and introspective figure, his deep thoughts hidden behind a veil of mystery. He navigates the complexities of life with a quiet determination, his presence commanding respect and intrigue from those around him.",
                    SpritesUrl = "Hirgh",
                    ItemRateType = 6,
                    ItemType = 0,
                    Price = 3000
                }
            };
            await _unitOfWork.BeginTransactionAsync();
            foreach (var newItem in tempData)
            {
                var gameItem = Mappers.GameMapper.GameItemRequestToGameItem(newItem);
                try
                {
                    await _unitOfWork.GameItemRepository.AddAsync(gameItem);
                    await _unitOfWork.SaveChangeAsync();
                }
                catch (Exception)
                {
                    await _unitOfWork.RollbackAsync();
                    throw;
                }
            }

            await _unitOfWork.CommitAsync();
        }
    }

    #region SHOPPING

    public async Task<List<int>> BuyItemFromShop(int idItem, int userId)
    {
        var boughtItem = await _unitOfWork.GameItemRepository.GetAsync(o => o.Id == idItem, null)
            .ContinueWith(o => o.Result.FirstOrDefault());

        if (boughtItem == null)
        {
            throw new BadRequestException("Item not found");
        }

        if (boughtItem.ItemType != ItemType.ShopItem)
        {
            throw new BadRequestException("Item bought not valid, must be a shop item");
        }
        if (boughtItem.ItemRateType == ItemRateType.Default)
        {
            throw new BadRequestException("Default Item, cannot buy");
        }
        var user = await _unitOfWork.GameUserProfileRepository.GetAsync(o => o.Id == userId, null)
            .ContinueWith(o => o.Result.FirstOrDefault());
        if (user == null)
        {
            throw new BadRequestException("User not found");
        }

        if (user.Coin < boughtItem.Price)
        {
            throw new BadRequestException("Not enough coin to buy this item");
        }

        var checkExisted = await _unitOfWork.ItemOwnedRepository.GetAsync(
                o => o.StudentId == user.StudentId && o.GameItemId == boughtItem.Id
                , null)
            .ContinueWith(o => o.Result.FirstOrDefault());

        if (checkExisted != null)
        {
            throw new BadRequestException("This item is already bought");
        }

        var newOwnedItem = new ItemOwned
        {
            Id = 0,
            DisplayName = boughtItem.ItemName,
            Quantity = 1,
            StudentId = user.StudentId,
            GameItemId = boughtItem.Id,
        };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            user.Coin -= boughtItem.Price;
            await _unitOfWork.ItemOwnedRepository.AddAsync(newOwnedItem);
            _unitOfWork.GameUserProfileRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();
            await _unitOfWork.CommitAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        return _unitOfWork.ItemOwnedRepository
            .GetAsync(o => o.StudentId == user.StudentId
                , null, includeProperties: nameof(GameItem))
            .ContinueWith(o => o.Result.Where(o => o.GameItem.ItemType == ItemType.ShopItem)).Result
            .Select(o => o.GameItemId).ToList();
    }

    public async Task<List<GameShopItem>> GetAllShopItem()
    {
        var result = await _unitOfWork.GameItemRepository
            .GetAsync(
                filter: o => o.ItemType == ItemType.ShopItem,
                orderBy: q => q.OrderBy(item => item.ItemRateType)
            );

        return result.Select(Mappers.GameMapper.GameItemToGameShopItem).ToList();
    }

    public async Task<PagingResponse<GameShopItem>> GetAllShopItem(int? page, int? size)
    {
        var result = await _unitOfWork.GameItemRepository
            .GetPaginateAsync(o => o.ItemType == ItemType.ShopItem, orderBy: q => q.OrderBy(item => item.ItemRateType),
                page, size);

        return new PagingResponse<GameShopItem>
        {
            TotalPages = result.TotalPages,
            TotalRecords = result.TotalRecords,
            Results = result.Results.Select(Mappers.GameMapper.GameItemToGameShopItem)
        };
    }

    #endregion

    #region GAME LEVEL

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

            result.Add(new ModeType()
            {
                IdMode = mode.Id,
                TypeName = mode.TypeName ?? "Null Name",
                totalLevel = 0
            });
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
            UserId = userData!.StudentId,
            DisplayName = userData.DisplayName,
            OldGem = userData.Gem,
            OldCoin = userData.Coin,
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
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        return result;
    }

    #endregion

    #region ADMIN SERVICES

    public async Task AddNewGameItem(NewItemRequest newItemRequest)
    {
        await _unitOfWork.BeginTransactionAsync();
        var gameItem = new GameItem
        {
            Id = 0,
            GameId = 0,
            ItemName = newItemRequest.ItemName,
            Details = newItemRequest.Details,
            SpritesUrl = newItemRequest.SpritesUrl,
            ItemRateType = (ItemRateType)newItemRequest.ItemRateType,
            ItemType = (ItemType)newItemRequest.ItemRateType,
            Price = newItemRequest.Price
        };

        try
        {
            await _unitOfWork.GameItemRepository.AddAsync(gameItem);
            await _unitOfWork.CommitAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

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
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        int gameLevelId;
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
        catch (Exception)
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
        catch (Exception)
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
                catch (Exception)
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
                catch (Exception)
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
        catch (Exception)
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
        catch (Exception)
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
            , includeProperties: $"{nameof(GameLevel.GameLevelType)}").ContinueWith(o => o.Result.ToList());

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
            GameLevelTypeName = gameLevel.GameLevelType.TypeName ?? "Not Found",
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

    public async Task<List<LevelDataResponse>> GetLevelsByMode(int modeId, int? page, int? size)
    {
        var query = await _unitOfWork.GameLevelRepository.GetPaginateAsync(
            o => o.GameLevelTypeId == modeId && o.LevelIndex != -1, null
            , includeProperties: $"{nameof(GameLevel.GameLevelType)}", page: page, size: size);

        if (!query.Results.Any())
        {
            throw new NotFoundException("Not found any level");
        }

        var result = query.Results.Select(gameLevel => new LevelDataResponse()
        {
            Id = gameLevel.Id,
            LevelIndex = gameLevel.LevelIndex ?? 0,
            CoinReward = gameLevel.CoinReward ?? 0,
            GemReward = gameLevel.GemReward ?? 0,
            VStartPosition = gameLevel.VStartPosition,
            GameLevelTypeId = gameLevel.GameLevelTypeId,
            GameLevelTypeName = gameLevel.GameLevelType.TypeName ?? "Not Found",
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
        catch (Exception)
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
        catch (Exception)
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