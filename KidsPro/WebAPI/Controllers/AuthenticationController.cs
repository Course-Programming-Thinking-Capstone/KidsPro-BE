using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.Account;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

/// <summary>
/// Authentication service for login and register 
/// </summary>
[ApiController]
[Route("api/v1/authentication")]
public class AuthenticationController : ControllerBase
{
    private IAccountService _accountService;

    public AuthenticationController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Phụ huynh đăng ký bằng email + password
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("register/email")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LoginAccountDto))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LoginAccountDto>> RegisterByEmailAsync(EmailRegisterDto dto)
    {
        var result = await _accountService.RegisterByEmailAsync(dto);
        return Created(nameof(RegisterByEmailAsync), result);
    }
}