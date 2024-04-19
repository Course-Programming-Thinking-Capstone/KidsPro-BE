using Application.Dtos.Response.CourseGame;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/coursegames")]
public class CourseGamesController : ControllerBase
{
    private ICourseGameService _courseGameService;

    public CourseGamesController(ICourseGameService courseGameService)
    {
        _courseGameService = courseGameService;
    }

    /// <summary>
    /// Get available course game on the system
    /// </summary>
    /// <returns></returns>
    [HttpGet("available")]
    [AllowAnonymous]
    public async Task<ActionResult<CourseGameDto>> GetAvailableCourseGameAsync()
    {
        var result = await _courseGameService.GetAvailableCourseGameAsync();
        return Ok(result);
    }
}