using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class TeacherProfile : BaseEntity
{
    [DataType(DataType.DateTime)]
    [Precision(2)]
    public DateTime FromDate { get; set; }

    [DataType(DataType.DateTime)]
    [Precision(2)]
    public DateTime ToDate { get; set; }

    [Required] public int TeacherId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;
}