using Application.Dtos.Request.Game;

namespace Application.Dtos.Request.Game;

public class ModifiedLevelDataRequest
{
    public int Id { get; set; }
    public int CoinReward { get; set; }
    public int GemReward { get; set; }
    public int VStartPosition { get; set; }
    public int GameLevelTypeId { get; set; }
    public List<LevelDetailRequest> LevelDetail { get; set; } = new();
}