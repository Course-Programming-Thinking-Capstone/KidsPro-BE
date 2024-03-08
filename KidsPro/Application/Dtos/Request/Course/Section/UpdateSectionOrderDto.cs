using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course.Section;

public record UpdateSectionOrderDto
{
    [Required] public int Id { get; set; }

    [Required] public int Order { get; set; }
}