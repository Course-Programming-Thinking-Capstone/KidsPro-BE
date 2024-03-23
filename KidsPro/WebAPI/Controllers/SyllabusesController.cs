using Application.Dtos.Request.Syllabus;
using Application.Dtos.Response.Syllabus;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constant = Application.Configurations.Constant;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/syllabuses")]
public class SyllabusesController : ControllerBase
{
    private ISyllabusService _syllabusService;

    public SyllabusesController(ISyllabusService syllabusService)
    {
        _syllabusService = syllabusService;
    }

    /// <summary>
    /// Admin create and post syllabus to system.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = $"{Constant.AdminRole}")]
    public async Task<ActionResult<SyllabusDetailDto>> CreateAsync([FromBody] CreateSyllabusDto dto)
    {
        var result = await _syllabusService.CreateAsync(dto);
        return Created(nameof(CreateAsync), result);
    }
}