using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.Account;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/games")]
public class GamesController : ControllerBase
{
    private IAccountService _accountService;

    public GamesController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Student login vào game
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("authentication/login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentGameLoginDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentGameLoginDto>> StudentGameLoginAsync([FromBody] EmailCredential request)
    {
        var result = await _accountService.StudentGameLoginAsync(request);
        return Ok(result);
    }
}