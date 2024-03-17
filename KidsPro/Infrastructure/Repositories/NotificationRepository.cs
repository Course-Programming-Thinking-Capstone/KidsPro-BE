using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class NotificationRepository:BaseRepository<Notification>, INotificationRepository
{
    public NotificationRepository(AppDbContext context, ILogger<BaseRepository<Notification>> logger) : base(context, logger)
    {
    }
}