using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Request.Course.Update.Lesson;

public record UpdateLessonDto
{
    public int? Id { get; init; }

    // [Required]
    [StringLength(250, ErrorMessage = "Name can not exceed 50 characters.")]
    public string? Name { get; init; } = null!;

    [Range(0, 1000)] public int? Duration { get; init; }

    public string? Content { get; init; }

    [StringLength(250, ErrorMessage = "Url can not exceed 250 character")]
    public string? ResourceUrl { get; set; }

    // [Required] 
    public LessonType? Type { get; init; }
}