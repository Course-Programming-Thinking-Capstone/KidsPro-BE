using System.ComponentModel.DataAnnotations;
using Application.Dtos.Request.Course.Quiz;
using Microsoft.EntityFrameworkCore;

namespace Application.Dtos.Request.Course.Update.Quiz;

public record UpdateQuestionDto
{
    public int? Id { get; init; }

    [MaxLength(750, ErrorMessage = "Title can not exceed 750 characters.")]
    // [Required(ErrorMessage = "Title is required.")]
    public string? Title { get; init; } = null!;

    [Precision(5, 2)]
    [Range(0, 100, ErrorMessage = "Score can not exceed 100.")]
    public decimal? Score { get; init; }

    public ICollection<UpdateOptionDto>? Options { get; init; }
}