using Application.Configurations;
using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.User;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/v1/authenticate")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginUserDto>> Register([FromBody] RegisterDto request)
    {
        var result = await _userService.RegisterAsync(request,3);

        /*
        // Create the response with a custom header
        var response = CreatedAtAction(nameof(Register), result.Item1);

        // Access the HttpContext to modify the headers
        HttpContext.Response.Headers.Add("AccessToken", result.Item2);
        HttpContext.Response.Headers.Add("RefreshToken", result.Item3);

        */
        return CreatedAtAction(nameof(Register), result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string phonenumber, string password)
    {
        var result = await _userService.LoginAsync(phonenumber, password);
        return Ok(result);
    }

    /// <summary>
    /// Reissue Token Including Access & Refesh Token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="refeshToken"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("reissue")]
    public IActionResult ReissueToken(string accessToken, string refeshToken, User user)
    {
        var result = _userService.ReissueToken(accessToken, refeshToken, user);
        if (result.Item1)
            return Ok(result);
        return NotFound(result.Item2);
    }
    /// <summary>
    /// Register cho thành viên nội bộ
    /// </summary>
    /// <param name="request"></param>
    /// <param name="role">1. Staff, 2. Teacher</param>
    /// <returns></returns>
    [Authorize(Constant.AdminRole)]
    [HttpPost("register/insider")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound,Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status409Conflict,Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type= typeof(ErrorDetail))]
    public async Task<ActionResult<LoginUserDto>> RegisterInsider([FromBody] RegisterDto request,RoleType role)
    {
        var result = new LoginUserDto();
        switch ((int)role)
        {
            // Register for staff
            case 1:
                result = await _userService.RegisterAsync(request, 1);
                break;
            // Register for teacher
            case 2:
                result = await _userService.RegisterAsync(request, 2);
                break;
            case 3:
                throw new BadRequestException("This API only create for staff & teacher role");
        }
         return CreatedAtAction(nameof(Register), result);
    }
}