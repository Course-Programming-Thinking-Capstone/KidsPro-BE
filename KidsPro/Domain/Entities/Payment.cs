using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(UserId), nameof(Type), IsUnique = true)]
public class Payment : BaseEntity
{
    [Required] public int UserId { get; set; }

    [Column(TypeName = "tinyint")] public PaymentType Type { get; set; }

    [MaxLength(250)] public string? Name { get; set; }

    [MaxLength(250)] public string AccountNumber { get; set; } = null!;

    public bool IsDefault { get; set; }

    public virtual User User { get; set; } = null!;
}