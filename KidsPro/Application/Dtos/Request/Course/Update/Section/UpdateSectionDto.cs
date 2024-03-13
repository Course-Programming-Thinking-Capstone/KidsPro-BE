using System.ComponentModel.DataAnnotations;
using Application.Dtos.Request.Course.Update.Quiz;
using UpdateLessonDto = Application.Dtos.Request.Course.Update.Lesson.UpdateLessonDto;

namespace Application.Dtos.Request.Course.Update.Section;

public record UpdateSectionDto
{
    [Required] public int Id { get; init; }

    public ICollection<UpdateLessonDto>? Lessons { get; init; }

    public ICollection<UpdateQuizDto>? Quizzes { get; init; }
}