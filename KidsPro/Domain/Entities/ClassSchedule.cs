using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class ClassSchedule : BaseEntity
{
    [MaxLength(250)] public string? RoomUrl { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime StartTime { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime EndTime { get; set; }
    
    public int Slot{ get; set; }
    public DayStatus StudyDay { get; set; } = DayStatus.NoDay;

    public virtual Class Class { get; set; } = null!;
    public int ClassId { get; set; }
}