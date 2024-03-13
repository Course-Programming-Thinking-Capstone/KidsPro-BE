using System.ComponentModel.DataAnnotations;
using Application.Configurations;
using Application.Dtos.Request.Authentication;
using Application.Dtos.Request.User;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Enums;
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

    /// <summary>
    /// Get account by id and role
    /// </summary>
    /// <param name="id"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("account/{id}")]
    public async Task<ActionResult<AccountDto>> GetByIdAsync([FromRoute] int id, [FromQuery] [Required] string role)
    {
        var result = await _accountService.GetAccountByIdAsync(id, role);
        return Ok(result);
    }

    /// <summary>
    /// Admin create account for staff and teacher. Gender is enum: "Male: 1, Female: 2".
    /// Role can be "Staff" or "Teacher".
    /// Default created account password is "0000"
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    //[Authorize(Roles = $"{Constant.AdminRole}")]
    [HttpPost("admin/account")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AccountDto))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<AccountDto>> CreateAccountAsync([FromBody] CreateAccountDto dto)
    {
        var result = await _accountService.CreateAccountAsync(dto);
        return Created(nameof(CreateAccountAsync), result);
    }

    /// <summary>
    /// Admin filter account. Gender is enum: "Male: 1, Female: 2".
    /// </summary>
    /// <param name="fullName"></param>
    /// <param name="gender"></param>
    /// <param name="status"></param>
    /// <param name="sortFullName"></param>
    /// <param name="sortCreatedDate"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole}")]
    [HttpGet("admin/account")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagingResponse<AccountDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<PagingResponse<AccountDto>>> FilterAccountAsync(
        [FromQuery] string? fullName,
        [FromQuery] Gender? gender,
        [FromQuery] string? status,
        [FromQuery] string? sortFullName,
        [FromQuery] string? sortCreatedDate,
        [FromQuery] int? page,
        [FromQuery] int? size
    )
    {
        var result = await _accountService.FilterAccountAsync(
            fullName,
            gender,
            status,
            sortFullName,
            sortCreatedDate,
            page,
            size
        );

        return Ok(result);
    }

   


}