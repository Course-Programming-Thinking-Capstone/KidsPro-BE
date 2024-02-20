using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Domain.Enums;

namespace Domain.Entities;

public class GameVersion : BaseEntity
{
    [Required]
    public int Version { get; set; }
    [Required]
    public GameModifierHistoryStatus Status { get; set; }
}