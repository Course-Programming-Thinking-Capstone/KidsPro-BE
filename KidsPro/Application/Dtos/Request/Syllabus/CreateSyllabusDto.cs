using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Syllabus;

public class CreateSyllabusDto
{
    [StringLength(250, ErrorMessage = "Syllabus name  can not exceed 250 character.")]
    [Required]
    public string Name { get; init; } = null!;

    [StringLength(1000, ErrorMessage = "Target can not exceed 1000 character.")]
    [Required]
    public string Target { get; init; } = null!;

    [Required(ErrorMessage = "Total slot is required.")]
    public int TotalSlot { get; init; }

    [Required(ErrorMessage = "Slot time is required.")]
    public int SlotTime { get; init; }

    [Range(0, 100, ErrorMessage = "Out of range 0 to 100")]
    public int? MinQuizScoreRatio { get; init; }

    public List<CreateSectionDto>? Sections { get; init; }

    public int TeacherId { get; init; }
}