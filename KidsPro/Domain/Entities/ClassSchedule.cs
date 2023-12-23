using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Generic;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class ClassSchedule : BaseEntity
{
    [StringLength(250)] public string? RoomUrl { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime StartTime { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime EndTime { get; set; }

    public ClassScheduleType Type { get; set; } = ClassScheduleType.Online;

    [StringLength(250)] public string? RecordUrl { get; set; }

    public int ClassId { get; set; }

    public Class Class { get; set; } = null!;
}