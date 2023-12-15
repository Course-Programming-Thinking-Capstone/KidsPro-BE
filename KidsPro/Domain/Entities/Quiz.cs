using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), nameof(LessonId), IsUnique = true)]
public class Quiz
{
    [Key]
    [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public int Id { get; set; }

    [Column(TypeName = "tinyint")]
    [Range(0, 255)]
    public int Order { get; set; }

    [Column(TypeName = "smallint")]
    [Range(0, 10000)]
    public int TotalQuestion { get; set; }

    [Range(0, int.MaxValue)] public decimal TotalScore { get; set; }

    [Range(0, int.MaxValue)] public decimal MinScore { get; set; }

    [StringLength(250)] public string? Title { get; set; }

    [StringLength(750)] public string? Description { get; set; }

    [Range(0, 6000)]
    [Column(TypeName = "smallint")]
    public int? Duration { get; set; }

    [Range(0, 255)]
    [Column(TypeName = "tinyint")]
    public int? NumberOfAttempt { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; } = DateTime.UtcNow;

    public Guid CreatedById { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    public int LessonId { get; set; }

    public Lesson Lesson { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}