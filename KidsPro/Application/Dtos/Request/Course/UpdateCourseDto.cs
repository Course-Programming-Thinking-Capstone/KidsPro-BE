using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course;

public record UpdateCourseDto
{
    [StringLength(250, ErrorMessage = "Class name can not exceed 250 character")]
    [Required(ErrorMessage = "Class name is required.")]
    public string? Name { get; init; } = null!;

    [StringLength(3000, ErrorMessage = "Description can not exceed 3000 character")]
    public string? Description { get; init; }

    public bool? IsFree { get; init; }
}