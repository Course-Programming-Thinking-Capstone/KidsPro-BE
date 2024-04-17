using Domain.Enums;
using Domain.Enums.Status;

namespace Application.Dtos.Response.Syllabus;

public class FilterSyllabusDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? CreatedDate { get; set; }

    public SyllabusStatus Status { get; set; }
}