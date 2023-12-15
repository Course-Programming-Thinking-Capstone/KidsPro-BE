using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(UserId), nameof(Type), IsUnique = true)]
public class Payment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public Guid UserId { get; set; }

    [Column(TypeName = "tinyint")] public PaymentType Type { get; set; }

    [MaxLength(250)] public string? Name { get; set; }

    [MaxLength(250)] public string AccountNumber { get; set; } = null!;

    public bool IsDefault { get; set; }

    public virtual User User { get; set; } = null!;
}