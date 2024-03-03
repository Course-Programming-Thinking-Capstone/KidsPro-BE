using Application.Dtos.Request.User;
using Application.Dtos.Response.Account;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

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
    /// <param name="request"></param>
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

    /// <summary>
    /// Đổi avatar
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPatch("account/avatar")]
    public async Task<ActionResult<string>> UpdateAvatarAsync([FromForm] IFormFile file)
    {
        var result = await _accountService.UpdatePictureAsync(file);
        return Ok(result);
    }

    /// <summary>
    /// Lấy thông tin của account hiện tại. Tùy role sẽ có data khác nhau
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("account")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<AccountDto>> GetCurrentAccountInformationAsync()
    {
        var result = await _accountService.GetCurrentAccountInformationAsync();
        return Ok(result);
    }
}