using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), nameof(CourseId), IsUnique = true)]
public class CourseSection: BaseEntity
{

    [Range(0, 255)]
    [Column(TypeName = "tinyint")]
    public int Order { get; set; }

    [StringLength(250)] public string Name { get; set; } = null!;

    public virtual int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}