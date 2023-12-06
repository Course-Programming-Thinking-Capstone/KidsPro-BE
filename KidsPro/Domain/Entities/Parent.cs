using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Parent
{
    [Key] public Guid UserId { get; set; }

    [Key] [MaxLength(20)] public string StudentId { get; set; } = null!;

    public bool IsDefault { get; set; } = false;

    [DataType(DataType.DateTime)]
    [Precision(2)]
    public DateTime CreatedDate { get; set; }

    public Guid? CreatedById { get; set; }

    public User? CreatedBy { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}