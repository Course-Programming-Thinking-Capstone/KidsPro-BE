using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), nameof(ChapterId), IsUnique = true)]
public class Lesson : BaseEntity
{
    [MaxLength(250)] public string Name { get; set; } = null!;

    [Column(TypeName = "tinyint")]
    [Range(0, 250)]
    public int Order { get; set; }
    
    [MaxLength(250)] public string ResourceUrl { get; set; } = null!;

    [Range(0, 1000)] public int? Duration { get; set; }

    public bool IsFree { get; set; }
    public bool IsRequired { get; set; }
    public bool IsDelete { get; set; }

    [Column(TypeName = "tinyint")]
    public LessonType Type { get; set; }

    public virtual Section Section { get; set; } = null!;
    public int ChapterId { get; set; }
    
}