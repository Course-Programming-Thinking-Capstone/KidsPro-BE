using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Domain.Enums;

namespace Domain.Entities;

public class GameLevel : BaseEntity
{
    public int LevelIndex { get; set; }
    public int CoinReward { get; set; }
    public int GemReward { get; set; }
    [Required]
    public GameLevelType GameLevelType { get; set; }
    [Range(1,53)]
    [Required]
    public int VStartPosition { get; set; } // 8 * (y-1) + x => position, example (1,1) => 8 * 0 + 1 = 1, (8,6) => 8*5 + 8 = 53
    
    public virtual ICollection<GameRockPosition> GameRockPositions { get; set; } = new List<GameRockPosition>();
    public virtual ICollection<GameBoardMapPosition> GameBoardMapPositions { get; set; } = new List<GameBoardMapPosition>();
    
}