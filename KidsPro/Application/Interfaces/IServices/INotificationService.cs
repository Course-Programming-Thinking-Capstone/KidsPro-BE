using Application.Dtos.Response.Notification;
using Application.Dtos.Response.Paging;

namespace Application.Interfaces.IServices;

public interface INotificationService
{
    Task<PagingResponse<NotificationDto>> GetAccountNotificationAsync(int? page, int? size);

    Task<NotificationDto> MarkNotificationAsReadAsync(int notificationId);

    Task<PagingResponse<NotificationDto>> MarkAllNotificationAsReadAsync(int? page, int? size);
    Task SendNotifyToAccountAsync(int toId, string title, string content);
}