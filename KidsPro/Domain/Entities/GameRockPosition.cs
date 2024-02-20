using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class GameRockPosition : BaseEntity
{
    [Required]
    public int GameLevelId { get; set; }
    [Range(1, 53)]
    [Required]
    public int VRockPosition { get; set; }

}