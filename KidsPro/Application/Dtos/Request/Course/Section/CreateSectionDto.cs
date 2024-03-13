using System.ComponentModel.DataAnnotations;
using Application.Dtos.Request.Course.Lesson;
using Application.Dtos.Request.Course.Quiz;

namespace Application.Dtos.Request.Course.Section;

public record CreateSectionDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(250, ErrorMessage = "Name can not exceed 2520 characters.")]
    public string Name { get; init; } = null!;

    // public int Order { get; init; }
    
    public ICollection<CreateLessonDto>? Lessons { get; set; }
    public ICollection<CreateQuizDto>? Quizzes { get; init; }

}