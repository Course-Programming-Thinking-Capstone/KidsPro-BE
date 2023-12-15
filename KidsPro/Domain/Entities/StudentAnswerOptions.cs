using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(StudentAnswerId), nameof(OptionId), IsUnique = true)]
public class StudentAnswerOptions
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(750)] public string? Content { get; set; }

    public int StudentAnswerId { get; set; }

    public virtual StudentAnswer StudentAnswer { get; set; } = null!;

    public int OptionId { get; set; }

    public virtual Option Option { get; set; } = null!;
}