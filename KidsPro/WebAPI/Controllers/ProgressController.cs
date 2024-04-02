using Application.Dtos.Response.StudentProgress;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/v1/progress")]
[ApiController]
public class ProgressController : ControllerBase
{
    private IProgressService _progress;

    public ProgressController(IProgressService progress)
    {
        _progress = progress;
    }

    /// <summary>
    /// Get student's section progress 
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("section")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<SectionProgressResponse>> GetSectionProgress(int studentId, int courseId)
    {
        var resutl = await _progress.GetProgressSection(studentId, courseId);
        return Ok(resutl);
    }
    
}