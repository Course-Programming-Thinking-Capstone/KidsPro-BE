using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Course : BaseEntity
{
    [MaxLength(250)] public string Name { get; set; } = null!;

    [MaxLength(3000)] public string? Description { get; set; }

    [Range(3, 20)]
    [Column(TypeName = "tinyint")]
    public int? FromAge { get; set; }

    [Range(3, 20)]
    [Column(TypeName = "tinyint")]
    public int? ToAge { get; set; }

    [MaxLength(3000)] public string? PreRequire { get; set; }

    [MaxLength(250)] public string? PictureUrl { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? StartSaleDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? EndSaleDate { get; set; }

    [Range(0, int.MaxValue)]
    [Precision(11, 2)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    [Precision(11, 2)]
    public decimal? DiscountPrice { get; set; }

    [Range(0, 1000)]
    [Column(TypeName = "smallint")]
    public int? TotalLesson { get; set; }

    [MaxLength(150)] public string? Language { get; set; }

    [MaxLength(250)] public string? GraduateCondition { get; set; }

    [Column(TypeName = "tinyint")] public CourseStatus Status { get; set; } = CourseStatus.Draft;

    public bool IsDelete { get; set; }
    
    public bool IsFree { get; set; } = false;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ModifiedDate { get; set; }

    public virtual Account CreatedBy { get; set; } = null!;
    public int CreatedById { get; set; }

    public virtual Account ModifiedBy { get; set; } = null!;
    public int ModifiedById { get; set; }

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual ICollection<CourseResource> CourseResources { get; set; } = new List<CourseResource>();
}