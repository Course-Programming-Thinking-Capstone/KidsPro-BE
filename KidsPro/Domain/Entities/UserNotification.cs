using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(NotificationId), nameof(AccountId), IsUnique = true)]
public class UserNotification : BaseEntity
{
    public bool IsRead { get; set; }

    public virtual Account Account { get; set; } = null!;
    public virtual int AccountId { get; set; }

    public virtual Notification Notification { get; set; } = null!;
    public int NotificationId { get; set; }
}