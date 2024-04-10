using System.ComponentModel.DataAnnotations;
using Application.Dtos.Request.Course.Update.Section;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Request.Course.Update.Course;

public record UpdateCourseDto
{
    [StringLength(1000, ErrorMessage = "description can not exceed 1000 character")]
    public string? Description { get; set; }

    public ICollection<UpdateSectionDto>? Sections { get; init; }
}