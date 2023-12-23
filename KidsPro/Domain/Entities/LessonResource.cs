using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;

namespace Domain.Entities;

public class LessonResource: BaseEntity
{

    [StringLength(250)] public string ResourceUrl { get; set; } = null!;

    [StringLength(750)] public string? Description { get; set; }

    [StringLength(250)] public string? Title { get; set; }

    public LessonResourceType Type { get; set; } = LessonResourceType.Video;

    public int LessonId { get; set; }

    public Lesson Lesson { get; set; } = null!;
}