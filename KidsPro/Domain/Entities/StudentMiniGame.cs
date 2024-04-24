using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class StudentMiniGame : BaseEntity
{
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime JoinTime { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime FinishTime { get; set; }

    public int StepCount { get; set; }

    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }

    public virtual MiniGame MiniGame { get; set; } = null!;
    public int MiniGameId { get; set; }
}