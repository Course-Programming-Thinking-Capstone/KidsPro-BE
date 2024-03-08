using Application.Dtos.Response.Game;

namespace Application.Interfaces.IServices;

public interface IGameService
{
    public Task<List<ModeType>> GetAllGameMode();
    public Task<List<CurrentLevelData>> GetUserCurrentLevel(int userId);
    public Task<LevelInformationResponse?> GetLevelInformation(int typeId, int levelIndex);
}