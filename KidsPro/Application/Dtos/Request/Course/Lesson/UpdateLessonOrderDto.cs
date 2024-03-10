using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course.Lesson;

public record UpdateLessonOrderDto
{
    [Required] public int LessonId { get; init; }

    [Required] public int Order { get; init; }
};