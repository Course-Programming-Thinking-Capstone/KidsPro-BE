using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(250)] public string Name { get; set; } = null!;

    [StringLength(3000)] public string? Description { get; set; }

    [StringLength(3000)] public string? Prerequisite { get; set; }

    [StringLength(250)] public string? PictureUrl { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? OpenDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? StartSaleDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? EndSaleDate { get; set; }

    [Range(0, float.MaxValue)] [Precision(11,2)] public decimal Price { get; set; }

    [Range(0, float.MaxValue)] [Precision(11,2)] public decimal DiscountPrice { get; set; }

    [Range(0, 2000)]
    [Column(TypeName = "smallint")]
    public int TotalLesson { get; set; }

    [Column(TypeName = "tinyint")] public CourseStatus Status { get; set; }

    public bool IsDelete { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    [Required] public Guid CreatedById { get; set; }

    public virtual User CreatedBy { get; set; } = null!;

    [Required]
    public Guid ModifiedById { get; set; }

    [ForeignKey(nameof(ModifiedById))] public virtual User ModifiedBy { get; set; } = null!;

    public virtual ICollection<CourseSection> CourseSections { get; set; } = new List<CourseSection>();
}