using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course;

public record UpdateCourseDto
{
    [StringLength(250, ErrorMessage = "Class name can not exceed 250 character")]
    [Required(ErrorMessage = "Class name is required.")]
    public string? Name { get; init; } = null!;

    [StringLength(3000, ErrorMessage = "Description can not exceed 3000 character")]
    public string? Description { get; init; }

    [StringLength(250, ErrorMessage = "Prerequisite can not exceed 250 character")]
    public string? Prerequisite { get; init; }

    [StringLength(150, ErrorMessage = "Language can not exceed 150 character")]
    public string? Language { get; init; }

    [StringLength(250, ErrorMessage = "Graduate condition can not exceed 250 character")]
    public string? GraduateCondition { get; init; }

    [Range(3, 30)] public int? FromAge { get; init; }

    [Range(3, 30)] public int? ToAge { get; init; }

    public bool? IsFree { get; init; }
}