using System.ComponentModel.DataAnnotations;
using Application.Dtos.Request.Course.Quiz;

namespace Application.Dtos.Request.Course.Update.Quiz;

public class UpdateQuizDto
{
    public int? Id { get; init; }

    [StringLength(250, ErrorMessage = "Title can not exceed 250 characters")]
    // [Required]
    public string? Title { get; init; } = null!;

    [StringLength(750, ErrorMessage = "description can not exceed 750 characters.")]
    public string? Description { get; init; }

    public int? Duration { get; init; }

    public int? NumberOfQuestion { get; init; }

    public bool? IsOrderRandom { get; init; }

    public int? NumberOfAttempt { get; init; }

    public ICollection<UpdateQuestionDto>? Questions { get; init; }
}