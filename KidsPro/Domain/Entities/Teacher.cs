using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(AccountId), IsUnique = true)]
public class Teacher : BaseEntity
{
    [MaxLength(150)] public string? Facebook { get; set; }

    [MaxLength(500)] public string? PersonalInformation { get; set; }

    [MaxLength(3000)] public string? Biography { get; set; }

    [MaxLength(250)] public string? ProfilePicture { get; set; }

    [MaxLength(11)] public string? PhoneNumber { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;

    public virtual ICollection<Class>? Classes { get; set; } = new List<Class>();
    public virtual ICollection<TeacherProfile> TeacherProfiles { get; set; } = new List<TeacherProfile>();
}