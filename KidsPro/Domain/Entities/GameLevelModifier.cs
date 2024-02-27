using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class GameLevelModifier:BaseEntity
{

    public virtual GameLevel GameLevel { get; set; } = null!;
    public int GameLevelId { get; set; }

    public virtual GameVersion Game { get; set; } = null!;
    public int GameId { get; set; }

    [MaxLength(750)]
    public string Description { get; set; } = null!;
}