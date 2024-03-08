using Application.Dtos.Request.Authentication;
using Application.Dtos.Request.Game;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Game;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/games")]
public class GamesController : ControllerBase
{
    private IAccountService _accountService;
    private IGameService _gameService;

    public GamesController(IAccountService accountService, IGameService gameService)
    {
        _accountService = accountService;
        _gameService = gameService;
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

    /// <summary>
    /// Get User Process
    /// </summary>
    /// <param name="id">User Id</param>
    /// <returns></returns>
    [HttpGet("userProcess/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrentLevelData))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CurrentLevelData>> GetCurrentLevelByUserId([FromRoute] int id)
    {
        var result = await _gameService.GetUserCurrentLevel(id);
        return Ok(result);
    }

    /// <summary>
    /// Get all mode that have in game
    /// </summary>
    /// <returns></returns>
    [HttpGet("gameMode")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModeType))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<ModeType>> GetAllGameMode()
    {
        var result = await _gameService.GetAllGameMode();
        return Ok(result);
    }

    /// <summary>
    ///  Get information of a level
    /// </summary>
    /// <param name="id">game mode id</param>
    /// <param name="index">level index</param>
    /// <returns></returns>
    [HttpGet("leveldata/{id}/{index}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LevelInformationResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LevelInformationResponse>> GetLevelData([FromRoute] int id, [FromRoute] int index)
    {
        var result = await _gameService.GetLevelInformation(id, index);
        return Ok(result);
    }

    /// <summary>
    /// User finish a level game
    /// </summary>
    [HttpPost("finishLevel")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LevelInformationResponse>> FinishLevelGame(
        [FromBody] UserFinishLevelRequest userFinishLevelRequest)
    {
        var result = await _gameService.UserFinishLevel(userFinishLevelRequest);
        return Ok(result);
    }
}