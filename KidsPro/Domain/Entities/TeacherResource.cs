using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;

namespace Domain.Entities;

public class TeacherResource:BaseEntity
{
    [MaxLength(250)] public string ResourceUrl { get; set; } = string.Empty;

    [Column(TypeName = "tinyint")] public TeacherResourceType Type { get; set; }

    [Required]
    public int TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;
}