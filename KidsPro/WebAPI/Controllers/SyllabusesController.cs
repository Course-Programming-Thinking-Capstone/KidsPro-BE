using Application.Dtos.Request.Syllabus;
using Application.Dtos.Response.Paging;
using Application.Dtos.Response.Syllabus;
using Application.Interfaces.IServices;
using Domain.Enums;
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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SyllabusDetailDto))]
    public async Task<ActionResult<SyllabusDetailDto>> CreateAsync([FromBody] CreateSyllabusDto dto)
    {
        var result = await _syllabusService.CreateAsync(dto);
        return Created(nameof(CreateAsync), result);
    }

    /// <summary>
    /// Admin filter syllabus on the system
    /// </summary>
    /// <param name="name"></param>
    /// <param name="status"></param>
    /// <param name="sortName"></param>
    /// <param name="sortCreatedDate"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagingResponse<FilterSyllabusDto>))]
    public async Task<ActionResult<PagingResponse<FilterSyllabusDto>>> FilterSyllabusAsync(
        [FromQuery] string? name,
        [FromQuery] SyllabusStatus? status,
        [FromQuery] string? sortName,
        [FromQuery] string? sortCreatedDate,
        [FromQuery] int? page,
        [FromQuery] int? size
    )
    {
        var result = await _syllabusService.FilterSyllabusAsync(name, status, sortName, sortCreatedDate, page, size);
        return Ok(result);
    }

    /// <summary>
    /// Get syllabus by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SyllabusDetailDto))]
    public async Task<ActionResult<SyllabusDetailDto>> GetByIsAsync([FromRoute] int id)
    {
        return await _syllabusService.GetByIdAsync(id);
    }
    
    /// <summary>
    /// Get number of syllabus that teacher has not created yet
    /// </summary>
    /// <returns></returns>
    [HttpGet("account/number-draft-syllabus")]
    [Authorize(Roles = $"{Constant.TeacherRole}")]
    public async Task<ActionResult<int>> GetNumberOfAccountDraftSyllabusAsync()
    {
        var result = await _syllabusService.GetNumberOfTeacherDraftSyllabusAsync();
        return Ok(result);
    }
}