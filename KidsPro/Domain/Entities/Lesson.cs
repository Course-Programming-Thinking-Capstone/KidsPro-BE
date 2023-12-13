using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), nameof(CourseSectionId), IsUnique = true)]
public class Lesson
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Order { get; set; }

    [Column(TypeName = "tinyint")] public LessonType Type { get; set; } = LessonType.Video;

    public bool IsFree { get; set; }

    public string? PictureUrl { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? OpenDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ClosedDate { get; set; }

    public bool IsDelete { get; set; }

    public int CourseSectionId { get; set; }

    public virtual CourseSection CourseSection { get; set; } = null!;

    public virtual ICollection<LessonResource> LessonResources { get; set; } = new List<LessonResource>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}