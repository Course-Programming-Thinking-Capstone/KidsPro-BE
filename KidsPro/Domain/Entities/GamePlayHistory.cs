using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class GamePlayHistory:BaseEntity
{
    public int LevelIndex { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime PlayTime { get; set; }
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime FinishTime { get; set; }
    
    public int GameLevelType { get; set; }
    
    public int Duration { get; set; }

    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }
}