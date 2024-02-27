using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class GameVersion : BaseEntity
{
    public int Version { get; set; }

    public int Status { get; set; }

    [MaxLength(750)] public string ChangeMessage { get; set; } = null!;
}