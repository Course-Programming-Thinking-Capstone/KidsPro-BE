using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class CurriculumResource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(TypeName = "tinyint")] public CurriculumResourceType Type { get; set; } = CurriculumResourceType.Resource;

    [StringLength(3000)] public string? Description { get; set; }

    [StringLength(250)] public string? ResourceUrl { get; set; }

    [StringLength(250)] public string? Title { get; set; }

    public int CurriculumId { get; set; }

    public virtual Curriculum Curriculum { get; set; } = null!;
}