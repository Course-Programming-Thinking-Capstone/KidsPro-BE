using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;
[Index(nameof(StudentId),nameof(QuizId), IsUnique = true)]
public class StudentQuiz:BaseEntity
{
    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;
    public int QuizId { get; set; }

    [Precision(5,2)]
    public decimal Score { get; set; }
    public int Attempt { get; set; }

    public int? Duration { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? StartTime { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? EndTime { get; set; }

    public bool IsPass { get; set; }

    public virtual ICollection<StudentOption> StudentAnswers { get; set; } = new List<StudentOption>();
}