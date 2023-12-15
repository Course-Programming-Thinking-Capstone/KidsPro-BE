using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(CurriculumId), nameof(CourseId))]
[Index(nameof(Order), IsUnique = true)]
public class CurriculumCourse
{
    public int CurriculumId { get; set; }

    public int CourseId { get; set; }

    [Column(TypeName = "tinyint")]
    [Range(0, 255)]
    public int Order { get; set; }

    public virtual Curriculum Curriculum { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}