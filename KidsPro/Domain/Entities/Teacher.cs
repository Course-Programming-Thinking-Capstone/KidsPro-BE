using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class Teacher : BaseEntity
{
    [MaxLength(150)] public string? Field { get; set; }

    [MaxLength(3000)] public string? Description { get; set; }

    public virtual ICollection<TeacherResource> TeacherResources { get; set; } = new List<TeacherResource>();

    public virtual ICollection<TeacherProfile> TeacherProfiles { get; set; } = new List<TeacherProfile>();

    public virtual ICollection<TeacherContactInformation> TeacherContactInformations { get; set; } =
        new List<TeacherContactInformation>();

    [Required] public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}