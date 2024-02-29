using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;

namespace Domain.Entities;

public class GameVersion : BaseEntity
{
    public int Version { get; set; }

    [Column(TypeName = "tinyint")] public GameVersionStatus Status { get; set; }

    [MaxLength(750)] public string ChangeMessage { get; set; } = null!;
}