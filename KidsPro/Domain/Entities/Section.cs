using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

// [Index(nameof(Order), nameof(CourseId), IsUnique = true)]
public class Section : BaseEntity
{
    [Column(TypeName = "tinyint")]
    [Range(0, 250)]
    public int Order { get; set; }

    [MaxLength(250)] public string Name { get; set; } = null!;
    public int SectionTime { get; set; }

    public virtual Course Course { get; set; } = null!;

    public int CourseId { get; set; }

    public virtual List<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual List<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual Game? Games { get; set; }
}