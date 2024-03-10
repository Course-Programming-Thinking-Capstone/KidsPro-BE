using Domain.Entities.Generic;

namespace Domain.Entities;

public class GameLevel : BaseEntity
{
    public int? LevelIndex { get; set; }

    public int? CoinReward { get; set; }
    public int? GemReward { get; set; }
    public int? Max { get; set; }

    public int VStartPosition { get; set; }

    public virtual LevelType GameLevelType { get; set; } = null!;
    public int GameLevelTypeId { get; set; }
}