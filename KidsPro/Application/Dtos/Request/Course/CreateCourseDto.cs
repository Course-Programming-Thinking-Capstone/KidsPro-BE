using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course;

public record CreateCourseDto
{
    [StringLength(250, ErrorMessage = "Class name can not exceed 3000 character")]
    [Required(ErrorMessage = "Class name is required.")]
    public string Name { get; set; } = null!;

    [StringLength(3000, ErrorMessage = "Description can not exceed 3000 character")]
    public string? Description { get; set; }

    [StringLength(3000, ErrorMessage = "Prerequisite can not exceed 3000 character")]
    public string? Prerequisite { get; set; }
    
    [Range(3, 100)]
    public int? FromAge { get; set; }
    
    [Range(3, 100)]
    public int? ToAge { get; set; }
}