using Application.Interfaces.IRepositories.Generic;
using Domain.Entities;

namespace Application.Interfaces.IRepositories;

public interface IUserNotificationRepository:IBaseRepository<UserNotification>
{
    Task MarkAllNotificationAsReadAsync(int accountId);
    
    Task<int> GetNumberOfAccountUnreadNotificationAsync(int accountId);
}