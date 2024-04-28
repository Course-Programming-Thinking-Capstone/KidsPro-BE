using Application.Dtos.Response;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/v1/dashboard")]
public class DashboardController : ControllerBase
{
    private IDashboardService _dashboard;
    private IQuizService _quiz;

    public DashboardController(IDashboardService dashboard, IQuizService quiz)
    {
        _dashboard = dashboard;
        _quiz = quiz;
    }

    /// <summary>
    /// Admin get dashboard
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<DashboardResponse>> AdminGetDashboardAsync()
    {
        var result = await _dashboard.GetDashboardAsync();
        return result;
    }
    [HttpGet("quiz")]
    public async Task<IActionResult> CheckAsyn()
    {
        await _quiz.RefeshNumberAttempt();
        return Ok();
    }
}