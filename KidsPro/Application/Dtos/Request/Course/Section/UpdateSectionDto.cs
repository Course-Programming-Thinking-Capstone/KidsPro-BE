using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course.Section;

public record UpdateSectionDto
{
    
    [MaxLength(250)]
    public string? Name { get; set; }
    
}