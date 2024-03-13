using Application.Dtos.Request.Course.Update.Section;

namespace Application.Dtos.Request.Course.Update.Course;

public record UpdateCourseDto
{
    public ICollection<UpdateSectionDto>? Sections { get; init; }
}