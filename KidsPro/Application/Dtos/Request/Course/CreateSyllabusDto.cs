using System.ComponentModel.DataAnnotations;
using Application.Dtos.Request.Course.Section;

namespace Application.Dtos.Request.Course;

public record CreateSyllabusDto
{
    [StringLength(250, ErrorMessage = "Class name can not exceed 250 character")]
    [Required(ErrorMessage = "Class name is required.")]
    public string Name { get; init; } = null!;

    [StringLength(1000, ErrorMessage = "CourseTarget can not exceed 1000 character")]
    public string? CourseTarget { get; init; }
    
    public int CourseSlot { get; init; }
    
    public int SlotTime { get; init; }
    
    [StringLength(500)]
    public string? Target { get; set; }

    public ICollection<CreateSectionDto>? Sections { get; init; }

    public int TeacherId { get; init; }
}