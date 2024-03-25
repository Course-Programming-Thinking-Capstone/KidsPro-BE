using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Code), IsUnique = true)]
public class Class : BaseEntity
{
    [MaxLength(20)] public string Code { get; set; } = null!;

    [Column(TypeName = "tinyint")]
    [Range(0, 250)]
    public int Duration { get; set; }

    public ClassStatus Status { get; set; } = ClassStatus.Active;

    public int? TotalSlot { get; set; }

    public int? TotalStudent { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    [Precision(2)]
    public DateTime? OpenDate { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    [Precision(2)]
    public DateTime? CloseDate { get; set; }
    public virtual Teacher? Teacher { get; set; } 
    public int? TeacherId { get; set; }

    public virtual Account CreatedBy { get; set; } = null!;
    public int CreatedById { get; set; }

    public virtual Course Course { get; set; } = null!;
    public int CourseId { get; set; }
    
    public virtual ICollection<StudentClass>? StudentsClasses { get; set; }
}