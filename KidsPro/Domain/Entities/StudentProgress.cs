using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(StudentId), nameof(CourseId))]
public class StudentProgress
{
    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }

    public virtual Course Course { get; set; } = null!;
    public int CourseId { get; set; }

    public StudentProgressStatus Status { get; set; } = StudentProgressStatus.OnGoing;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime EnrolledDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? CompletedDate { get; set; }

    public virtual Section Section { get; set; } = null!;
    public int SectionId { get; set; }
}