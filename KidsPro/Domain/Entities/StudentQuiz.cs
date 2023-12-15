using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(StudentCode), nameof(QuizId), IsUnique = true)]
public class StudentQuiz
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)] public decimal Score { get; set; }

    [Column(TypeName = "tinyint")]
    [Range(0, 255)]
    public int Attempt { get; set; } = 1;

    public bool IsPass { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [StringLength(20)] public string StudentCode { get; set; } = null!;

    [ForeignKey(nameof(StudentCode))] public virtual Student Student { get; set; } = null!;

    public int QuizId { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;


    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}