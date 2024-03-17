using System.ComponentModel.DataAnnotations;
using Application.Dtos.Request.Course.Update.Section;

namespace Application.Dtos.Request.Course.Update.Course;

public record UpdateCourseDto
{
    [StringLength(3000, ErrorMessage = "Description can not exceed 3000 character")]
    public string? Description { get; set; }
    public ICollection<UpdateSectionDto>? Sections { get; init; }
}