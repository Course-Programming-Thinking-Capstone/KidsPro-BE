using Application.Dtos.Response.Notification;
using Application.Dtos.Response.Paging;
using Application.Utils;
using Domain.Entities;

namespace Application.Mappers;

public static class NotificationMapper
{
    public static NotificationDto UserNotificationToNotificationDto(UserNotification entity)
        => new NotificationDto()
        {
            Id = entity.Id,
            IsRead = entity.IsRead,
            Title = entity.Notification.Title,
            Content = entity.Notification.Content,
            Date = DateUtils.FormatDateTimeToDatetimeV1(entity.Notification.Date),
        };

    public static PagingResponse<NotificationDto> UserNotificationToNotificationDto(
        PagingResponse<UserNotification> entities)
        => new PagingResponse<NotificationDto>()
        {
            TotalPages = entities.TotalPages,
            TotalRecords = entities.TotalRecords,
            Results = entities.Results.Select(UserNotificationToNotificationDto).ToList()
        };
}