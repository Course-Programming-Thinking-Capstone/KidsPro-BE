using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(UserId), nameof(StudentId))]
public class Parent
{
    public Guid UserId { get; set; }

    public string StudentId { get; set; } = null!;

    public bool IsDefault { get; set; } = false;

    [DataType(DataType.DateTime)]
    [Precision(2)]
    public DateTime CreatedDate { get; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}