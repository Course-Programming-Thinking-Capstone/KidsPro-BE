using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(AccountId), IsUnique = true)]
public class Admin : BaseEntity
{
    public int AccountId { get; set; }

    public Account Account { get; set; } = null!;
}