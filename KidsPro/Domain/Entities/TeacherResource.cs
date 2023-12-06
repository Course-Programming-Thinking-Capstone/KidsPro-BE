using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class TeacherResource
{
    [Key] public int Id { get; set; }

    [MaxLength(250)] public string ResourceUrl { get; set; } = string.Empty;

    [Column(TypeName = "tinyint")] public TeacherResourceType Type { get; set; }

    public Guid TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;
}