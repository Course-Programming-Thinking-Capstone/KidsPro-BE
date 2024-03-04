using System.ComponentModel.DataAnnotations;
using Application.Dtos.Request.Course;

namespace Application.Dtos.Request.Old.Course;

public class UpdateCourseDto
{
    
    [StringLength(250, ErrorMessage = "Class name can not exceed 3000 character")]
    public string? Name { get; set; } 

    [StringLength(3000, ErrorMessage = "Description can not exceed 3000 character")]
    public string? Description { get; set; }

    [StringLength(3000, ErrorMessage = "Prerequisite can not exceed 3000 character")]
    public string? Prerequisite { get; set; }
    
    [Range(3, 100)]
    public int? FromAge { get; set; }
    
    [Range(3, 100)]
    public int? ToAge { get; set; }
    
    public List<AddCourseResourceDto>? Resources { get; set; }
}