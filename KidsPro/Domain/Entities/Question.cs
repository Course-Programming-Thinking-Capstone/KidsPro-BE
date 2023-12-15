using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), nameof(QuizId), IsUnique = true)]
public class Question
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Range(0, 255)]
    [Column(TypeName = "tinyint")]
    public int Order { get; set; }

    [StringLength(750)] public string Title { get; set; } = null!;

    [StringLength(250)] public string? ResourceUrl { get; set; }

    [Range(0, int.MaxValue)] [Precision(5,2)] public decimal Score { get; set; }

    [Column(TypeName = "tinyint")] public QuizQuestionType Type { get; set; } = QuizQuestionType.Radio;

    public int QuizId { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual ICollection<Option> Options { get; set; } = new List<Option>();
}