using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), nameof(QuizId), IsUnique = true)]
public class Question : BaseEntity
{
    [Range(1, 100)] public int Order { get; set; }
    [MaxLength(750)] public string Title { get; set; } = null!;
    [Precision(5,2)]
    public decimal Score { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;
    public int QuizId { get; set; }

    public virtual ICollection<Option> Options { get; set; } = new List<Option>();
}