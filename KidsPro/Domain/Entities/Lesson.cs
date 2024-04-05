using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

// [Index(nameof(Order), nameof(SectionId), IsUnique = true)]
public class Lesson : BaseEntity
{
    [MaxLength(250)] public string Name { get; set; } = null!;

    [Column(TypeName = "tinyint")]
    [Range(0, 250)]
    public int Order { get; set; }

    [MaxLength(250)] public string? ResourceUrl { get; set; }

    [Column(TypeName = "nvarchar(max)")] public string? Content { get; set; }

    [Range(0, 1000)] public int? Duration { get; set; }

    public bool IsFree { get; set; }

    [Column(TypeName = "tinyint")] public LessonType Type { get; set; }

    public virtual Section Section { get; set; } = null!;
    public int SectionId { get; set; }

    public virtual ICollection<StudentLesson>? StudentLessons { get; set; }

    public virtual ICollection<Quiz>? Quizzes { get; set; }
}