using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(StudentId), nameof(ClassId))]
public class StudentClass
{
    public int StudentId { get; set; }

    public int ClassId { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime JoinDate { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(StudentId))] public virtual Student Student { get; set; } = null!;

    [ForeignKey(nameof(ClassId))] public virtual Class Class { get; set; } = null!;
}