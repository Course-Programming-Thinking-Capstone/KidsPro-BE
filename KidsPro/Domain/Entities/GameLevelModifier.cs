using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class GameLevelModifier: BaseEntity
{
    [Required]
    public int GameVersionId { get; set; }
    [Required]
    public int GameLevelId { get; set; }
    [StringLength(750)]
    public string? Description { get; set; }
}