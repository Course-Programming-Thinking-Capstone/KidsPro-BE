using Application.Dtos.Response.Course.Lesson;
using Domain.Entities;

namespace Application.Dtos.Response.Course;

public class SectionDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }

    public int CourseId { get; set; }

    public ICollection<LessonDto>? Lessons { get; set; }
    
}