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
        var result = await _userService.RegisterAsync(request,4);

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
    /// Reissue Token Including Access and Refesh Token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="refeshToken"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("reissue/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status501NotImplemented, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<IActionResult> ReissueToken(string accessToken, string refeshToken,[FromRoute] int id)
    {
        var result =await _userService.ReissueToken(accessToken, refeshToken, id);
        if (result.Item1)
            return Ok(new
            {
                AccessToken=result.Item2,
                RefeshToken=result.Item3
            });
        return NotFound(result.Item1);
    }
    /// <summary>
    /// Register cho thành viên nội bộ
    /// </summary>
    /// <param name="request"></param>
    /// <param name="role">2. Staff, 3. Teacher</param>
    /// <returns></returns>
    //[Authorize(Constant.AdminRole)]
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
             //Register for admin
            case 1:
                result = await _userService.RegisterAsync(request, 1);
                break;
             // Register for staff
            case 2:
                result = await _userService.RegisterAsync(request, 2);
                break;
            // Register for teacher
            case 3:
                result = await _userService.RegisterAsync(request, 3);
                break;
            case 4:
                throw new BadRequestException("This API only create for staff & teacher role");
        }
         return CreatedAtAction(nameof(Register), result);
    }
}