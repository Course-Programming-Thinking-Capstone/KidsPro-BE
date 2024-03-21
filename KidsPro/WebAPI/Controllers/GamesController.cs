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
    [HttpGet("setup/init-database")]
    public async Task<ActionResult> InitDatabase()
    {
        await _gameService.InitDatabase();
        return Ok();
    }

    #region GAME CLIENT

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
    [HttpGet("user-process/{id}")]
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
    [HttpGet("game-mode")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ModeType))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<ModeType>> GetAllGameMode()
    {
        var result = await _gameService.GetAllGameMode();
        return Ok(result);
    }

    /// <summary>
    /// Get information of a level
    /// </summary>
    /// <param name="id">game mode id</param>
    /// <param name="index">level index</param>
    /// <returns></returns>
    [HttpGet("level-data/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LevelInformationResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LevelInformationResponse>> GetLevelData([FromQuery] int id, [FromQuery] int index)
    {
        var result = await _gameService.GetLevelInformation(id, index);
        return Ok(result);
    }

    /// <summary>
    /// User finish a level game, return new user coin if first time clear level
    /// </summary>
    [HttpPost("finish-level")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LevelInformationResponse>> FinishLevelGame(
        [FromBody] UserFinishLevelRequest userFinishLevelRequest)
    {
        var result = await _gameService.UserFinishLevel(userFinishLevelRequest);
        return Ok(result);
    }

    #endregion

    #region Admin API

    /// <summary>
    /// Get Level information by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("game-level/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LevelDataResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LevelDataResponse>> GetLevelById([FromRoute] int id)
    {
        var result = await _gameService.GetLevelDataById(id);
        return Ok(result);
    }

    /// <summary>
    /// Get Level information by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("game-mode/{modeId}/game-level")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LevelDataResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<List<LevelDataResponse>>> GetLevelsByGameMode([FromRoute] int modeId)
    {
        var result = await _gameService.GetLevelsByMode(modeId);
        return Ok(result);
    }

    /// <summary>
    /// Admin add a new game level to game
    /// </summary>
    /// <returns></returns>
    [HttpPost("game-level")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult> AddNewLevels(
        [FromBody] ModifiedLevelDataRequest modifiedLevelData)
    {
        await _gameService.AddNewLevel(modifiedLevelData);
        return Ok();
    }

    /// <summary>
    /// Admin add a new game level to game
    /// </summary>
    /// <returns></returns>
    [HttpPut("game-level")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult> UpdateLevel(
        [FromBody] ModifiedLevelDataRequest modifiedLevelData)
    {
        await _gameService.UpdateLevel(modifiedLevelData);
        return Ok();
    }

    #endregion
}