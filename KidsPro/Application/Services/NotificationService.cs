using System.Linq.Expressions;
using Application.Dtos.Response.Notification;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Application.Mappers;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class NotificationService : INotificationService
{
    private IUnitOfWork _unitOfWork;
    private IAuthenticationService _authenticationService;
    private ILogger<AccountService> _logger;

    public NotificationService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,
        ILogger<AccountService> logger)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public async Task<PagingResponse<NotificationDto>> GetAccountNotificationAsync(int? page, int? size)
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out _);

        var parameter = Expression.Parameter(typeof(UserNotification));
        Expression filter = Expression.Constant(true); // default is "where true"

        //set default page size
        if (!page.HasValue || !size.HasValue)
        {
            page = 1;
            size = 10;
        }

        IOrderedQueryable<UserNotification> OrderBy(IQueryable<UserNotification> q) =>
            q.OrderByDescending(n => n.Notification.Date);

        try
        {
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(UserNotification.AccountId)),
                    Expression.Constant(accountId)));

            var entities = await _unitOfWork.UserNotificationRepository.GetPaginateAsync(
                filter: Expression.Lambda<Func<UserNotification, bool>>(filter, parameter),
                orderBy: OrderBy,
                includeProperties: $"{nameof(UserNotification.Notification)}",
                page: page,
                size: size
            );
            var result = NotificationMapper.UserNotificationToNotificationDto(entities);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute {methodName} method: \nDetail: {errorDetail}.",
                nameof(this.GetAccountNotificationAsync), e.Message);
            throw new Exception($"Error when execute {nameof(this.GetAccountNotificationAsync)} method");
        }
    }

    public async Task<NotificationDto> MarkNotificationAsReadAsync(int notificationId)
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out _);

        var notification = await _unitOfWork.UserNotificationRepository.GetByIdAsync(notificationId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Notification {notificationId} not found."));

        if (notification.AccountId != accountId)
            throw new ForbiddenException("Owner account is require for this feature.");

        if (!notification.IsRead)
        {
            notification.IsRead = true;

            _unitOfWork.UserNotificationRepository.Update(notification);
            await _unitOfWork.SaveChangeAsync();
        }

        return NotificationMapper.UserNotificationToNotificationDto(notification);
    }

    public async Task<PagingResponse<NotificationDto>> MarkAllNotificationAsReadAsync(int? page, int? size)
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out _);

        await _unitOfWork.UserNotificationRepository.MarkAllNotificationAsReadAsync(accountId);
        await _unitOfWork.SaveChangeAsync();

        //Filter user notification
        var parameter = Expression.Parameter(typeof(UserNotification));
        Expression filter = Expression.Constant(true); // default is "where true"

        //set default page size
        if (!page.HasValue || !size.HasValue)
        {
            page = 1;
            size = 10;
        }

        IOrderedQueryable<UserNotification> OrderBy(IQueryable<UserNotification> q) =>
            q.OrderByDescending(n => n.Notification.Date);

        try
        {
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(UserNotification.AccountId)),
                    Expression.Constant(accountId)));

            var entities = await _unitOfWork.UserNotificationRepository.GetPaginateAsync(
                filter: Expression.Lambda<Func<UserNotification, bool>>(filter, parameter),
                orderBy: OrderBy,
                includeProperties: $"{nameof(UserNotification.Notification)}",
                page: page,
                size: size
            );
            var result = NotificationMapper.UserNotificationToNotificationDto(entities);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when execute {methodName} method: \nDetail: {errorDetail}.",
                nameof(this.GetAccountNotificationAsync), e.Message);
            throw new Exception($"Error when execute {nameof(this.GetAccountNotificationAsync)} method");
        }
    }
    
    public async Task SendNotifyToAccountAsync(int toId, string title, string content)
    {
        var account = await _unitOfWork.AccountRepository.GetByIdAsync(toId)
                      ?? throw new BadRequestException($"AccountId: {toId} not found");

        var userNotifications = new List<UserNotification>()
        {
            new()
            {
                AccountId = toId,
                IsRead = false,
            }
        };

        var notify = new Notification()
        {
            Title = title,
            Content = content,
            Date = DateTime.UtcNow,
            UserNotifications = userNotifications
        };

        await _unitOfWork.NotificationRepository.AddAsync(notify);
        if (await _unitOfWork.SaveChangeAsync() < 0)
            throw new NotImplementedException("Add Notify Failed");
    }

    public async Task<int> GetNumberOfAccountUnreadNotificationAsync()
    {
        _authenticationService.GetCurrentUserInformation(out var accountId, out _);
        var result =  await _unitOfWork.UserNotificationRepository.GetNumberOfAccountUnreadNotificationAsync(accountId);
        return result;
    }
}