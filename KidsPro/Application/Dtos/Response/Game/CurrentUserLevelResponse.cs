using Domain.Enums;

namespace Application.Dtos.Response.Game
{
    public class CurrentUserLevelResponse
    {
        private List<CurrentLevelData> Values { get; set; }
    }

    public class CurrentLevelData
    {
        private GameMode Mode { get; set; }
        private int LevelIndex { get; set; }
    }
}