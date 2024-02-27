using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class LevelType : BaseEntity
{
    [MaxLength(50)] public string? TypeName { get; set; } = null!;
}