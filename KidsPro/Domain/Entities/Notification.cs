using System.ComponentModel.DataAnnotations;
using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public class Notification : BaseEntity
{
    [MaxLength(500)] public string NotificationMessage { get; set; } = null!;

    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
    [Precision(2)]
    public DateTime Date { get; set; }

    public bool IsRead { get; set; }

    public Account Account { get; set; } = null!;
    public int AccountId { get; set; }
}