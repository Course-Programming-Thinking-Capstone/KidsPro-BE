using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), nameof(QuestionId), IsUnique = true)]
public class Answer : BaseEntity
{
    public int Order { get; set; }

    [MaxLength(750)] public string Content { get; set; } = null!;
    [MaxLength(750)] public string? AnswerExplain { get; set; }
    public bool IsCorrect { get; set; }

    public virtual Question Question { get; set; } = null!;
    public int QuestionId { get; set; }
}