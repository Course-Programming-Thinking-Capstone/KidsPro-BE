using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course.Section;

public record UpdateSectionComponentNumberDto
{
    [Required] public string Name { get; set; } = null!;

    [Required] public int MaxNumber { get; set; }
};