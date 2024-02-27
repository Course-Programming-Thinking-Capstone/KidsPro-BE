using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Order), nameof(SectionId), IsUnique = true)]
public class Quiz : BaseEntity
{
    [Range(1, 100)] public int Order { get; set; }
    [Range(0, 100)] public int TotalQuestion { get; set; }

    public int TotalScore { get; set; }

    public int MinScore { get; set; }

    [MaxLength(250)] public string Title { get; set; } = null!;

    [MaxLength(750)] public string? Description { get; set; }

    public int? Duration { get; set; }

    public int NumberOfQuestion { get; set; }

    public bool IsOrderRandom { get; set; }

    public int? NumberOfAttempt { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CreatedDate { get; set; }

    public virtual Teacher CreatedBy { get; set; } = null!;
    public int CreatedById { get; set; }

    public virtual Section Section { get; set; } = null!;
    public int SectionId { get; set; }


    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}