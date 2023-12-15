using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class TeacherProfile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [DataType(DataType.DateTime)]
    [Precision(2)]
    public DateTime FromDate { get; set; }

    [DataType(DataType.DateTime)]
    [Precision(2)]
    public DateTime ToDate { get; set; }

    [Required]
    public Guid TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;
}