using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course.Update.Quiz;

public record UpdateOptionDto
{
    public int? Id { get; init; }
    
    [MaxLength(750, ErrorMessage = "Content can not exceed 750 characters.")]
    // [Required(ErrorMessage = "Content is required.")]
    public string Content { get; init; } = null!;

    [MaxLength(750, ErrorMessage = "Answer explain can not exceed 750 characters.")]
    public string? AnswerExplain { get; init; }

    public bool? IsCorrect { get; init; }
}