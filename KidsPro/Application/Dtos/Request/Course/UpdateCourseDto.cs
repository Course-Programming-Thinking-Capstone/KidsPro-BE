using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course;

public record UpdateCourseDto
{
    [StringLength(250, ErrorMessage = "Class name can not exceed 250 character")]
    [Required(ErrorMessage = "Class name is required.")]
    public string? Name { get; init; } = null!;

    public bool? IsFree { get; init; }
}