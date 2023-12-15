using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), IsUnique = true)]
public class CurriculumCourse
{
    [Key, Column(Order = 0)] public int CurriculumId { get; set; }

    [Key, Column(Order = 1)] public int CourseId { get; set; }

    [Column(TypeName = "tinyint")]
    [Range(0, 255)]
    public int Order { get; set; }

    public virtual Curriculum Curriculum { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}