using System.ComponentModel.DataAnnotations;
using Application.Dtos.Request.Course.Section;

namespace Application.Dtos.Request.Course;

public record CreateCourseDto
{
    [StringLength(250, ErrorMessage = "Class name can not exceed 250 character")]
    [Required(ErrorMessage = "Class name is required.")]
    public string Name { get; set; } = null!;

    [StringLength(3000, ErrorMessage = "Description can not exceed 3000 character")]
    public string? Description { get; set; }
    
    public ICollection<CreateSectionDto>? Sections { get; init; } 
}