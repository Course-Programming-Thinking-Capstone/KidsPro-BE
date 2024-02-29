using Application.Dtos.Request.User;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private IAccountService _accountService;

    public UsersController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Đổi mật khẩu
    /// </summary>
    /// <param name="oldPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    [HttpPatch("account/password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
    public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto request)
    {
        await _accountService.ChangePasswordAsync(request);
        return Ok();
    }

    [Authorize]
    [HttpPatch("account/avatar")]
    public async Task<ActionResult> UpdateAvatarAsync([FromForm] IFormFile file)
    {
        await _accountService.UpdatePictureAsync(file);
        return Ok();
    }
}