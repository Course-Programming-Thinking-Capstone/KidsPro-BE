using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class ClassSchedule : BaseEntity
{
    [MaxLength(250)] public string? RoomUrl { get; set; }

    [DataType(DataType.Time)]
    public TimeSpan StartTime { get; set; }

    [DataType(DataType.Time)]
    public TimeSpan EndTime { get; set; }
    
    public int Slot{ get; set; }
    public DayStatus StudyDay { get; set; } = DayStatus.NoDay;

    public virtual Class Class { get; set; } = null!;
    public int ClassId { get; set; }
}