using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), nameof(CourseSectionId), IsUnique = true)]
public class Lesson : BaseEntity
{
    public int Order { get; set; }

    [StringLength(250)] public string Name { get; set; } = null!;

    [Column(TypeName = "tinyint")] public LessonType Type { get; set; } = LessonType.Video;

    public bool IsFree { get; set; }

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
}