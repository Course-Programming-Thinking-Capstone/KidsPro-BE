using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course.Section;

public record CreateSectionDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(250)]
    public string Name { get; set; } = null!;

    public int Order { get; set; }

}