using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;

public class LessonResource
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(250)] public string ResourceUrl { get; set; } = null!;

    [StringLength(750)] public string? Description { get; set; }

    [StringLength(250)] public string? Title { get; set; }

    public LessonResourceType Type { get; set; } = LessonResourceType.Video;

    public int LessonId { get; set; }

    public Lesson Lesson { get; set; } = null!;
}