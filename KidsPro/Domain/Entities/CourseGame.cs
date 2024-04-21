using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums.Status;

namespace Domain.Entities;

public class CourseGame : BaseEntity
{
    [Required] [StringLength(250)] public string Name { get; set; } = null!;

    [StringLength(250)] public string Url { get; set; } = null!;

    [Column(TypeName = "tinyint")] public CourseGameStatus Status { get; set; } = CourseGameStatus.Active;

    public bool IsDelete { get; set; }

    // public int? CourseId { get; set; }
    // public virtual Course? Course { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}