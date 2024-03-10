using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course.Lesson;

public record UpdateLessonDto
{
    [StringLength(250)] public string? Name { get; init; } = null!;

    [Range(0, 1000)] public int? Duration { get; init; }

    public bool? IsFree { get; init; }
};