using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Application.Dtos.Request.Course.Quiz;

public record CreateQuestionDto
{
    [MaxLength(750, ErrorMessage = "Title can not exceed 750 characters.")]
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; init; } = null!;

    [Precision(5, 2)]
    [Range(0, 100, ErrorMessage = "Score can not exceed 100.")]
    public decimal? Score { get; init; }

    public ICollection<CreateOptionDto> Options { get; init; } = null!;
}