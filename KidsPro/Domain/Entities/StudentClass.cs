using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class StudentClass
{
    [Key, Column(Order = 0)] public string StudentId { get; set; } = null!;

    [Key, Column(Order = 1)] public string ClassCode { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime JoinDate { get; set; } = DateTime.UtcNow;

    public virtual Student Student { get; set; } = null!;

    [ForeignKey(nameof(ClassCode))] public virtual Class Class { get; set; } = null!;
}