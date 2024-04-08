using Application.Interfaces.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class UserNotificationRepository : BaseRepository<UserNotification>, IUserNotificationRepository
{
    public UserNotificationRepository(AppDbContext context, ILogger<BaseRepository<UserNotification>> logger) : base(
        context, logger)
    {
    }

    public override async Task<UserNotification?> GetByIdAsync(int id, bool disableTracking = false)
    {
        IQueryable<UserNotification> query = _dbSet;

        if (disableTracking)
        {
            query.AsNoTracking();
        }

        return await _dbSet.Include(un => un.Notification)
            .FirstOrDefaultAsync(un => un.Id == id);
    }

    public async Task MarkAllNotificationAsReadAsync(int accountId)
    {
        await _dbSet.Where(un => un.AccountId == accountId && !un.IsRead)
            .ExecuteUpdateAsync(setters => setters.SetProperty(un => un.IsRead, true));
    }

    public async Task<int> GetNumberOfAccountUnreadNotificationAsync(int accountId)
    {
        return await _dbSet.Where(un => un.AccountId == accountId && !un.IsRead)
            .CountAsync();
    }
}