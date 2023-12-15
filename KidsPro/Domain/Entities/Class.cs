using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class Class
{
    [Key] public string Code { get; set; } = null!;

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

    [Required] public Guid TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;

    [Required] public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();

    public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
}