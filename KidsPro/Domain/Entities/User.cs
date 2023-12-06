using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class User
{
    [Key] public Guid Id { get; set; }

    [MaxLength(100)] public string FullName { get; set; } = string.Empty;
    [MaxLength(11)] public string PhoneNumber { get; set; } = string.Empty;
    [MaxLength(150)] public string PasswordHash { get; set; } = string.Empty;
    [MaxLength(250)] public string? PictureUrl { get; set; }
    [Column(TypeName = "tinyint")] public Gender? Gender { get; set; }
    [Column(TypeName = "tinyint")] public UserStatus Status { get; set; } = UserStatus.User;
    public bool IsDelete { get; set; } = false;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual Teacher? Teacher { get; set; }

    public virtual ICollection<Parent>? Parents { get; set; }

    public virtual ICollection<Payment>? Payments { get; set; }
}