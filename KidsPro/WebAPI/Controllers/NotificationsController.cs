using Application.Dtos.Response.Notification;
using Application.Dtos.Response.Paging;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/notifications")]
public class NotificationsController : ControllerBase
{
    private INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Get notification of current user
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet("account")]
    [Authorize]
    public async Task<ActionResult<PagingResponse<NotificationDto>>> GetAccountNotificationAsync([FromQuery] int? page,
        [FromQuery] int? size)
    {
        var result = await _notificationService.GetAccountNotificationAsync(page, size);
        return Ok(result);
    }
    
    /// <summary>
    /// Get number of unread notification 
    /// </summary>
    /// <returns></returns>
    [HttpGet("account/number-of-unread")]
    [Authorize]
    public async Task<ActionResult<int>> GetNumberOfAccountUnreadNotificationAsync()
    {
        var result = await _notificationService.GetNumberOfAccountUnreadNotificationAsync();
        return Ok(result);
    }

    /// <summary>
    /// Mark notification as read
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("account/{id:int}")]
    [Authorize]
    public async Task<ActionResult<NotificationDto>> MarkNotificationAsReadAsync([FromRoute] int id)
    {
        var result = await _notificationService.MarkNotificationAsReadAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Mark all notification as read
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpPatch("account")]
    [Authorize]
    public async Task<ActionResult<PagingResponse<NotificationDto>>> MarkAllNotificationAsReadAsync(
        [FromQuery] int? page, [FromQuery] int? size)
    {
        var result = await _notificationService.MarkAllNotificationAsReadAsync(page, size);
        return Ok(result);
    }
}