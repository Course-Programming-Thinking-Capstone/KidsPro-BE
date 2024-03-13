using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Dtos.Request.Course.Lesson;

public record CreateLessonDto
{
    [Required]
    [StringLength(250, ErrorMessage = "Name can not exceed 50 characters.")]
    public string Name { get; init; } = null!;

    [Required]
    [Range(0, 250, ErrorMessage = "Order can not exceed 250.")]
    public int Order { get; init; }

    [Range(0, 1000)] public int? Duration { get; init; }

    public string? Content { get; init; }

    [StringLength(250, ErrorMessage = "Url can not exceed 250 character")]
    public string? ResourceUrl { get; init; }

    [Required] public LessonType Type { get; init; }

    public bool IsFree { get; init; }
};