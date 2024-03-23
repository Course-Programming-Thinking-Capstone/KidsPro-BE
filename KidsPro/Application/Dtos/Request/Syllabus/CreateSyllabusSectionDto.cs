using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Syllabus;

public class CreateSyllabusSectionDto
{
    [Required]
    [StringLength(250, ErrorMessage = "Name exceed 250 character.")]
    public string Name { get; init; } = null!;
}