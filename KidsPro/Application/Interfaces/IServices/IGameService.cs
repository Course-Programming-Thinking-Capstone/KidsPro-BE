using Application.Dtos.Request.Game;
using Application.Dtos.Response.Game;

namespace Application.Interfaces.IServices;

public interface IGameService
{
    public Task InitDatabase();
    public Task<List<ModeType>> GetAllGameMode();
    public Task<List<CurrentLevelData>> GetUserCurrentLevel(int userId);
    public Task<LevelInformationResponse?> GetLevelInformation(int typeId, int levelIndex);
    public Task<int> UserFinishLevel(UserFinishLevelRequest userFinishLevelRequest);
    public Task AddNewLevel(ModifiedLevelDataRequest modifiedLevelData, bool onTransaction = true);
    public Task<List<LevelDataResponse>> GetLevelsByMode(int modeId);
    public Task<LevelDataResponse> GetLevelDataById(int id);
    public Task UpdateLevel(ModifiedLevelDataRequest modifiedLevelData);
    public Task UpdateLevelIndex(ModifiedLevelIndex modifiedLevelData);
    public Task SoftDeleteLevelGame(int id);
}