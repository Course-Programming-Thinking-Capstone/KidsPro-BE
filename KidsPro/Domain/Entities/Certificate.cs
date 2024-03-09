using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(StudentId), nameof(CourseId))]
public class Certificate
{
    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }

    public virtual Course Course { get; set; } = null!;
    public int CourseId { get; set; }

    [MaxLength(250)] public string ResourceUrl { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime CompletionDate { get; set; }

    [StringLength(750)] public string? Description { get; set; }

}