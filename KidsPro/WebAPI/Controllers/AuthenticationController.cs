using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.User;
using Application.Interfaces.Services;
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
        var result = await _userService.RegisterAsync(request);

        // Create the response with a custom header
        var response = CreatedAtAction(nameof(Register), result.Item1);

        // Access the HttpContext to modify the headers
        HttpContext.Response.Headers.Add("AccessToken", result.Item2);
        HttpContext.Response.Headers.Add("RefreshToken", result.Item3);
        return response;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string phonenumber, string password)
    {
        var result = await _userService.LoginAsync(phonenumber, password);
        // Create the response with a custom header
        var response = Ok(result.Item1);

        // Access the HttpContext to modify the headers
        HttpContext.Response.Headers.Add("AccessToken", result.Item2);
        HttpContext.Response.Headers.Add("RefreshToken", result.Item3);
        return response;
    }
}