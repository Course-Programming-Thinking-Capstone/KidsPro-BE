using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Code), IsUnique = true)]
public class Class : BaseEntity
{
    public string Code { get; set; } = null!;

    [StringLength(250)] public string Name { get; set; } = null!;

    [Range(0, 10000)]
    [Column(TypeName = "smallint")]
    public int Duration { get; set; }

    [Column(TypeName = "tinyint")] public ClassStatus Status { get; set; } = ClassStatus.Active;

    [Range(0, 10000)]
    [Column(TypeName = "smallint")]
    public int TotalSlot { get; set; }

    [Range(0, 10000)]
    [Column(TypeName = "smallint")]
    public int TotalStudent { get; set; }

    [Required] public int TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;

    [Required] public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();

    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
}