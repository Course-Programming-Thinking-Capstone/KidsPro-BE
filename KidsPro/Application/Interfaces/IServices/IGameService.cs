using Application.Dtos.Request.Game;
using Application.Dtos.Response.Game;
using Application.Dtos.Response.Paging;

namespace Application.Interfaces.IServices;

public interface IGameService
{
    public Task InitDatabase();
    public Task<List<ModeType>> GetAllGameMode();
    public Task<List<CurrentLevelData>> GetUserCurrentLevel(int userId);
    public Task<LevelInformationResponse?> GetLevelInformation(int typeId, int levelIndex);
    public Task<UserDataResponse> UserFinishLevel(UserFinishLevelRequest userFinishLevelRequest);
    public Task AddNewLevel(ModifiedLevelDataRequest modifiedLevelData);
    public Task<List<LevelDataResponse>> GetLevelsByMode(int modeId);

    public Task<List<LevelDataResponse>> GetLevelsByMode(int modeId, int? page,
        int? size);

    public Task<LevelDataResponse> GetLevelDataById(int id);
    public Task UpdateLevel(ModifiedLevelDataRequest modifiedLevelData);
    public Task UpdateLevelIndex(ModifiedLevelIndex modifiedLevelData);
    public Task SoftDeleteLevelGame(int id);
    public Task<List<GameItem>> GetAllShopItem();
    public Task<PagingResponse<GameItem>> GetAllShopItem(int? page, int? size);
    public Task<BuyResponse> BuyItemFromShop(int idItem, int userId);
    public Task<List<int>> GetUserShopItem(int userId);
}