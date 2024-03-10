using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Dtos.Request.Course.Lesson;

public record CreateLessonDto
{
    [Required] [StringLength(250)] public string Name { get; set; } = null!;

    [Required] [Range(0, 250)] public int Order { get; set; }

    [Range(0, 1000)] public int? Duration { get; set; }

    public bool IsFree { get; set; }
};