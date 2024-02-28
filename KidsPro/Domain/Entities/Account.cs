using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Email), IsUnique = true)]
public class Account : BaseEntity
{
    [MaxLength(100)] public string Email { get; set; } = null!;

    [MaxLength(50)] public string FullName { get; set; } = null!;

    [MaxLength(150)] public string PasswordHash { get; set; } = null!;
    [MaxLength(150)] public string? PictureUrl { get; set; } 

    [Column(TypeName = "tinyint")] public UserStatus Status { get; set; } = UserStatus.Active;

    public bool IsDelete { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    //Account
    public virtual Admin? Admin { get; set; }

    //Teacher 
    public virtual Teacher? Teacher { get; set; }

    //Staff
    public virtual Staff? Staff { get; set; }

    //Parent
    public virtual Parent? Parent { get; set; }

    //Student
    public virtual Student? Student { get; set; }
}