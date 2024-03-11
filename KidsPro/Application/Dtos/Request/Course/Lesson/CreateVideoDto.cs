using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course.Lesson;

public record CreateVideoDto : CreateLessonDto
{
    [StringLength(250)] public string? ResourceUrl { get; init; }
};