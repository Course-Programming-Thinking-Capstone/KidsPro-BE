namespace Application.Dtos.Request.Game;

public class UserFinishLevelRequest
{
    public int UserID { get; set; }
    public int ModeId { get; set; }
    public int LevelIndex { get; set; }
    public DateTime StartTime { get; set; }
}