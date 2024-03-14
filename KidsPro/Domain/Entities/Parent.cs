using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(PhoneNumber), IsUnique = true)]
public class Parent : BaseEntity
{
    [MaxLength(11)] public string? PhoneNumber { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public virtual ICollection<GameVoucher>? Vouchers { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = null!;

}