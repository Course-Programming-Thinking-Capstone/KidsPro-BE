using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class GameBoardMapPosition : BaseEntity
{
    [Required]
    public int GameLevelId { get; set; }
    [Range(1,53)]
    [Required]
    public int VBoardPosition { get; set; } 
}