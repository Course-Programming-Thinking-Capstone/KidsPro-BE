using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Curriculum: BaseEntity
{

    [StringLength(250)] public string Name { get; set; } = null!;

    [StringLength(250)] public string? PictureUrl { get; set; }

    [StringLength(3000)] public string? Description { get; set; }
    
    [Range(3, 100)]
    [Column(TypeName = "tinyint")]
    public int? FromAge { get; set; }

    [Range(3, 100)]
    [Column(TypeName = "tinyint")]
    public int? ToAge { get; set; }

    public int TotalStudent { get; set; }

    [Column(TypeName = "tinyint")] public CurriculumStatus Status { get; set; } = CurriculumStatus.Draft;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? OpenDate { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? PostedDate { get; set; }

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

    [Range(0, 255)]
    [Column(TypeName = "tinyint")]
    public int TotalCourse { get; set; } = 0;

    public bool IsDelete { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime? ApprovedDate { get; set; }

    [Required] public int CreatedById { get; set; }
    [ForeignKey(nameof(CreatedById))] public virtual User CreatedBy { get; set; } = null!;

    [Required] public int ModifiedById { get; set; }

    [ForeignKey(nameof(ModifiedById))] public virtual User ModifiedBy { get; set; } = null!;

    public int? ApprovedById { get; set; }

    [ForeignKey(nameof(ApprovedById))] public virtual User? ApprovedBy { get; set; }

    public virtual ICollection<CurriculumResource> CurriculumResources { get; set; } = new List<CurriculumResource>();

    public virtual ICollection<CurriculumCourse> CurriculumCourses { get; set; } = new List<CurriculumCourse>();
}