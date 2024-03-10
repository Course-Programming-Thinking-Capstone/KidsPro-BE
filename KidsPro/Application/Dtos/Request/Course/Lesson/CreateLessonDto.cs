using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Dtos.Request.Course.Lesson;

public record CreateLessonDto
{
    [Required] [StringLength(250)] public string Name { get; init; } = null!;

    [Required] [Range(0, 250)] public int Order { get; init; }

    [Range(0, 1000)] public int? Duration { get; init; }

    public bool IsFree { get; init; }
};