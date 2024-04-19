using Application.Configurations;
using Application.Dtos.Request.Authentication;
using Application.Dtos.Request.Game;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Game;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
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

    #region SHOP & Item

    /// <summary>
    /// Admin Delete game item
    /// </summary>
    /// <returns></returns>
    [HttpDelete("game-item/")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<PagingResponse<GameItemResponse>>> UpdateNewGameItem(
        [FromRoute] int deleteId)
    {
        await _gameService.DeleteGameItem(deleteId);
        return Ok();
    }

    /// <summary>
    /// Admin update game item
    /// </summary>
    /// <returns></returns>
    [HttpPut("game-item/")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<PagingResponse<GameItemResponse>>> UpdateNewGameItem(
        [FromBody] NewItemRequest newItemRequest)
    {
        await _gameService.UpdateGameItem(newItemRequest);
        return Ok();
    }

    /// <summary>
    /// Admin Add new game item
    /// </summary>
    /// <returns></returns>
    [HttpPost("game-item/")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<PagingResponse<GameItemResponse>>> AddNewGameItem(
        [FromBody] NewItemRequest newItemRequest)
    {
        await _gameService.AddNewGameItem(newItemRequest);
        return Ok();
    }

    /// <summary>
    /// Admin get all drop able item in game
    /// </summary>
    /// <returns></returns>
    [HttpGet("game-item/pagination")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagingResponse<GameItemResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<PagingResponse<GameItemResponse>>> GetGameItemPagination(
        [FromQuery] int page,
        [FromQuery] int size)
    {
        var result = await _gameService.GetGameItemPagination(page, size);
        return Ok(result);
    }

    /// <summary>
    /// Get user Items
    /// </summary>
    /// <returns></returns>
    [HttpGet("game-drop-item-owned/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GameItemResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<List<GameItemResponse>>> GetUserItem([FromRoute] int userId)
    {
        var result = await _gameService.GetUserItem(userId);
        return Ok(result);
    }

    /// <summary>
    /// Get pagination of item
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet("shop-item/pagination")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagingResponse<GameItemResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<PagingResponse<GameItemResponse>>> GetShopItemPagination(
        [FromQuery] int page,
        [FromQuery] int size)
    {
        var result = await _gameService.GetAllShopItem(page, size);
        return Ok(result);
    }

    /// <summary>
    /// Get all of shop item
    /// </summary>
    /// <returns></returns>
    [HttpGet("shop-item/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GameItemResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<List<GameItemResponse>>> GetShopItem()
    {
        var result = await _gameService.GetAllShopItem();
        return Ok(result);
    }

    /// <summary>
    /// User buy item from shop
    /// </summary>
    /// <param name="itemId">item id</param>
    /// <param name="userId">user id</param>
    /// <returns>List of owned shop item</returns>
    [Authorize(Roles = $"{Constant.StudentRole},{Constant.AdminRole},")]
    [HttpPost("game-shop-item-owned/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BuyResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<BuyResponse>> UserBuyItem([FromQuery] int itemId,
        [FromQuery] int userId)
    {
        var result = await _gameService.BuyItemFromShop(itemId, userId);
        return Ok(result);
    }

    /// <summary>
    /// Get user item owned
    /// </summary>
    /// <returns>List of owned shop item</returns>
    [Authorize(Roles = $"{Constant.StudentRole},{Constant.AdminRole},")]
    [HttpGet("game-shop-item-owned/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<int>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<List<int>>> UserBuyItem([FromQuery] int userId)
    {
        var result = await _gameService.GetUserShopItem(userId);
        return Ok(result);
    }

    #endregion

    #region GAME CLIENT

    /// <summary>
    /// Student login vào game
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("authentication/login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentGameLoginDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentGameLoginDto>> StudentGameLoginAsync([FromBody] StudentLoginRequest request)
    {
        var result = await _accountService.StudentGameLoginAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Get User Process
    /// </summary>
    /// <param name="id">User Id</param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.StudentRole},{Constant.AdminRole},")]
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
    [Authorize(Roles = $"{Constant.StudentRole},{Constant.AdminRole},")]
    [HttpPost("game-play-history")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDataResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<UserDataResponse>> FinishLevelGame(
        [FromBody] UserFinishLevelRequest userFinishLevelRequest)
    {
        var result = await _gameService.UserFinishLevel(userFinishLevelRequest);
        return Ok(result);
    }

    #endregion

    #region Admin API

    /// <summary>
    /// Admin Get Levels by game mode id
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},")]
    [HttpGet("game-mode/{modeId}/game-level")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LevelDataResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<List<LevelDataResponse>>> GetLevelsByGameMode([FromRoute] int modeId,
        [FromQuery] int? page,
        [FromQuery] int? size)
    {
        if (page == null || size == null)
        {
            var result = await _gameService.GetLevelsByMode(modeId);
            return Ok(result);
        }
        else
        {
            var result = await _gameService.GetLevelsByMode(modeId, page, size);
            return Ok(result);
        }
    }

    /// <summary>
    /// Admin Get Level detail by level id
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},")]
    [HttpGet("game-level/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LevelDataResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LevelDataResponse>> GetLevelById([FromRoute] int id)
    {
        var result = await _gameService.GetLevelDataById(id);
        return Ok(result);
    }

    /// <summary>
    /// Admin add a new game level to game
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},")]
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
    /// Admin update an level
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},")]
    [HttpPut("game-level")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult> UpdateLevel(
        [FromBody] ModifiedLevelDataRequest modifiedLevelData)
    {
        await _gameService.UpdateLevel(modifiedLevelData);
        return Ok();
    }

    /// <summary>
    /// Admin remove an level
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},")]
    [HttpDelete("game-level/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult> DeleteLevel(
        [FromRoute] int id)
    {
        await _gameService.SoftDeleteLevelGame(id);
        return Ok();
    }

    /// <summary>
    /// Admin update index of level
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},")]
    [HttpPut("game-level-index")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult> UpdateLevelIndex(
        [FromBody] ModifiedLevelIndex modifiedLevelData)
    {
        await _gameService.UpdateLevelIndex(modifiedLevelData);
        return Ok();
    }

    #endregion
}