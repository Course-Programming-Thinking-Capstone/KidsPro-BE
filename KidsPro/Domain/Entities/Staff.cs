using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class Staff : BaseEntity
{
    [MaxLength(3000)] public string? Biography { get; set; }

    [MaxLength(250)] public string? ProfilePicture { get; set; }

    [MaxLength(11)] public string? PhoneNumber { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public int CreatedById { get; set; }
    public virtual Admin CreatedBy { get; set; } = null!;
}