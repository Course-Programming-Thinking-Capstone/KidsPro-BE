using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course;

public record AddCourseResourceDto()
{
    [StringLength(3000, ErrorMessage = "Max length is 3000 character")]
    public string? Description { get; set; }

    [StringLength(250, ErrorMessage = "Max length is 250 character")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Resource url is required.")]
    [StringLength(250, ErrorMessage = "Max length is 250 character")]
    public string ResourceUrl { get; set; } = string.Empty;
}