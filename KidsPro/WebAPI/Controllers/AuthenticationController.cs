using Application.Configurations;
using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.Account;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Enums;
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
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register/email")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LoginAccountDto))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LoginAccountDto>> RegisterByEmailAsync(EmailRegisterDto request)
    {
        var result = await _accountService.RegisterByEmailAsync(request);
        return Created(nameof(RegisterByEmailAsync), result);
    }

    // /// <summary>
    // /// Phụ huynh đăng ký bằng sdt + password
    // /// </summary>
    // /// <param name="request"></param>
    // /// <returns></returns>
    // [HttpPost("register/phoneNumber")]
    // [AllowAnonymous]
    // [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LoginAccountDto))]
    // [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetail))]
    // public async Task<ActionResult<LoginAccountDto>> RegisterByPhoneNumberAsync(PhoneNumberRegisterDto request)
    // {
    //     var result = await _accountService.RegisterByPhoneNumberAsync(request);
    //     return Created(nameof(RegisterByEmailAsync), result);
    // }

    /// <summary>
    /// UserName trên hệ thống đăng nhập bằng email + password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login/email")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginAccountDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LoginAccountDto>> LoginByEmailAsync(EmailCredential request)
    {
        var result = await _accountService.LoginByEmailAsync(request);
        if (result.Status == UserStatus.NotActivated.ToString())
            return BadRequest(new
            {
                AccountStatus=result.Status,
                Token=result.AccessToken,
                Meesage="The Account has not been activated, " +
                        "please click on the link sent in the email to activate the account"
            });
        return Ok(result);
    }

    // /// <summary>
    // /// Phụ huynh có thể đăng nhập bằng số điện thoại đã đăng ký
    // /// </summary>
    // /// <param name="request"></param>
    // /// <returns></returns>
    // [HttpPost("login/phoneNumber")]
    // [AllowAnonymous]
    // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginAccountDto))]
    // [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    // public async Task<ActionResult<LoginAccountDto>> LoginByPhoneNumberAsync(PhoneCredential request)
    // {
    //     var result = await _accountService.LoginByPhoneNumberAsync(request);
    //     return Ok(result);
    // }
    
    /// <summary>
    /// Khi nhấn link confirm bên email, client gọi api này để xác thực activate account
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    //[Authorize(Roles = $"{Constant.ParentRole},{Constant.StudentRole},{Constant.StaffRole},{Constant.TeacherRole},")]
    [HttpGet("confirm/check/{code}")]
    public async Task<IActionResult> CheckConfirmation(string code)
    {
        await _accountService.CheckConfirmation(code);
        return Ok("The account has been successfully activated");
    }
   
    /// <summary>
    /// Sent confirmation code again, Request login authorize
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole},{Constant.StudentRole},{Constant.StaffRole},{Constant.TeacherRole},")]
    [HttpGet("confirm/send")]
    public async Task<IActionResult> SendConfirmation()
    {
        await _accountService.SendConfirmation();
        return Ok("Sent confirmation code successful");
    }
}