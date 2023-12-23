using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Gmail), IsUnique = true)]
[Index(nameof(Code), IsUnique = true)]
public class Student : BaseEntity
{
    [MaxLength(20)] public string Code { get; set; } = string.Empty;

    [MaxLength(100)] public string Username { get; set; } = string.Empty;

    [MaxLength(150)] public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(50)] public string? Gmail { get; set; }

    [Column(TypeName = "tinyint")] public Gender? Gender { get; set; }

    [MaxLength(250)] public string? PictureUrl { get; set; }

    [Column(TypeName = "smallint")]
    [Range(1900, 2155)]
    public int? YearOfBirth { get; set; }

    [MaxLength(100)] public string FullName { get; set; } = string.Empty;

    [Column(TypeName = "tinyint")] public StudentStatus Status { get; set; }

    public bool IsDelete { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}