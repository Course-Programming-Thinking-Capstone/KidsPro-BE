using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;

namespace Domain.Entities;

public class CourseResource : BaseEntity
{
    [MaxLength(250)] public string ResourceUrl { get; set; } = null!;

    [MaxLength(3000)] public string? Description { get; set; }

    [StringLength(250)] public string? Title { get; set; }

    public virtual Course Course { get; set; } = null!;
    public int CourseId { get; set; }
}