using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), nameof(LessonId), IsUnique = true)]
public class Quiz: BaseEntity
{

    [Column(TypeName = "tinyint")]
    [Range(0, 255)]
    public int Order { get; set; }

    [Column(TypeName = "smallint")]
    [Range(0, 10000)]
    public int TotalQuestion { get; set; }

    [Range(0, int.MaxValue)]
    [Precision(5, 2)]
    public decimal TotalScore { get; set; }

    [Range(0, int.MaxValue)]
    [Precision(5, 2)]
    public decimal MinScore { get; set; }

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
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Required] public int CreatedById { get; set; }

    [ForeignKey(nameof(CreatedById))] public virtual User CreatedBy { get; set; } = null!;

    public int LessonId { get; set; }

    public Lesson Lesson { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}