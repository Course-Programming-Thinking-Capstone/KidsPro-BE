using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constant = Application.Configurations.Constant;
using CourseDto = Application.Dtos.Response.Old.Course.CourseDto;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/courses")]
public class CoursesController : ControllerBase
{
    private ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    /// <summary>
    /// Get by course Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> GetByIdAsync([FromRoute] int id)

    {
        var result = await _courseService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Create course 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> CreateCourseAsync([FromBody] CreateCourseDto dto)
    {
        var result = await _courseService.CreateCourseAsync(dto);
        return Created(nameof(CreateCourseDto), result);
    }

    /// <summary>
    /// Update course 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> UpdateCourseAsync([FromRoute] int id, [FromBody] UpdateCourseDto dto)
    {
        var result = await _courseService.UpdateCourseAsync(id, dto);
        return Ok(result);
    }

    /// <summary>
    /// Update course picture
    /// </summary>
    /// <param name="id"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    [HttpPatch("{id}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    public async Task<ActionResult<CourseDto>> UpdateCoursePictureAsync([FromRoute] int id, [FromForm] IFormFile file)
    {
        var result = await _courseService.UpdateCoursePictureAsync(id, file);
        return Ok(result);
    }

    /// <summary>
    /// Create course section.
    /// id in route is course's Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    [HttpPost("{id}/section")]
    public async Task<ActionResult<SectionDto>> CreateSectionAsync([FromRoute] int id, [FromBody] CreateSectionDto dto)
    {
        var result = await _courseService.CreateSectionAsync(id, dto);
        return Created(nameof(CreateSectionAsync), result);
    }

    /// <summary>
    /// Update section name 
    /// </summary>
    /// <param name="sectionId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    [HttpPatch("section/{sectionId}")]
    public async Task<ActionResult<SectionDto>> UpdateSectionAsync([FromRoute] int sectionId,
        [FromBody] UpdateSectionDto dto)
    {
        var result = await _courseService.UpdateSectionAsync(sectionId, dto);
        return Ok(result);
    }

    /// <summary>
    /// Update section order
    /// </summary>
    /// <param name="sectionId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    [HttpPatch("section/{sectionId}")]
    public async Task<ActionResult<SectionDto>> UpdateSectionOrderAsync([FromRoute] int sectionId,
        [FromBody] List<UpdateSectionOrderDto> dto)
    {
        var result = await _courseService.UpdateSectionOrderAsync(sectionId, dto);
        return Ok(result);
    }
}