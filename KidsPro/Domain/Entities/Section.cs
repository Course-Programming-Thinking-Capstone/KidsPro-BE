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

    public virtual Course Course { get; set; } = null!;

    public int CourseId { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}