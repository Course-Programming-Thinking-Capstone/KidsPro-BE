using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;

namespace Domain.Entities;

public class TeacherContactInformation : BaseEntity
{
    [MaxLength(250)] public string Url { get; set; } = string.Empty;

    [Column(TypeName = "tinyint")] public ContactInformationType Type { get; set; }

    [Required] public int TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;
}