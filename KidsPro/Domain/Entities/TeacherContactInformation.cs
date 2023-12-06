using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class TeacherContactInformation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(250)] public string Url { get; set; } = string.Empty;

    [Column(TypeName = "tinyint")] public ContactInformationType Type { get; set; }

    public Guid TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;
}