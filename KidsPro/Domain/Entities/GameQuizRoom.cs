using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class GameQuizRoom : BaseEntity
{
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime StartTime { get; set; }

    public int Duration { get; set; }

    [MaxLength(10)] public string JoinCode { get; set; } = null!;

    public virtual GameLevel GameLevel { get; set; } = null!;
    public int GameLevelId { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;
    public int TeacherId { get; set; }
}