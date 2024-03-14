using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class UserNotificationRepository : BaseRepository<UserNotification>, IUserNotificationRepository
{
    public UserNotificationRepository(AppDbContext context, ILogger<BaseRepository<UserNotification>> logger) : base(
        context, logger)
    {
    }
}