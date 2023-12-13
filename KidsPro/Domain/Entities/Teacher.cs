using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Teacher
{
    [Key] public Guid Id { get; set; }

   [MaxLength(150)] public string? Field { get; set; }

    [MaxLength(3000)] public string? Description { get; set; }

    public virtual ICollection<TeacherResource> TeacherResources { get; set; } = new List<TeacherResource>();

    public virtual ICollection<TeacherProfile> TeacherProfiles { get; set; } = new List<TeacherProfile>();

    public virtual ICollection<TeacherContactInformation> TeacherContactInformations { get; set; } =
        new List<TeacherContactInformation>();

    public Guid UserId { get; set; }
    
    public virtual User User { get; set; } = null!;
}