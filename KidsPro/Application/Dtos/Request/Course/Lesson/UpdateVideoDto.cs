using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course.Lesson;

public record UpdateVideoDto : UpdateLessonDto
{
    [StringLength(250)] public string? ResourceUrl { get; init; }
};