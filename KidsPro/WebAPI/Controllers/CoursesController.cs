using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.FilterCourse;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constant = Application.Configurations.Constant;
using CourseDto = Application.Dtos.Response.Old.Course.CourseDto;
using UpdateCourseDto = Application.Dtos.Request.Course.Update.Course.UpdateCourseDto;

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
    [HttpGet("{id:int}")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> GetByIdAsync([FromRoute] int id)

    {
        var result = await _courseService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Admin, staff, teacher filter course on the system
    /// </summary>
    /// <param name="name"></param>
    /// <param name="status"></param>
    /// <param name="sortName"></param>
    /// <param name="action"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="sortCreatedDate"></param>
    /// <param name="sortModifiedDate"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagingResponse<FilterCourseDto>))]
    public async Task<ActionResult<IPagingResponse<FilterCourseDto>>> FilterCourseAsync(
        [FromQuery] string? name,
        [FromQuery] CourseStatus? status,
        [FromQuery] string? sortName,
        [FromQuery] string? sortCreatedDate,
        [FromQuery] string? sortModifiedDate,
        [FromQuery] string? action,
        [FromQuery] int? page,
        [FromQuery] int? size)
    {
        var result = await _courseService.FilterCourseAsync(
            name,
            status,
            sortName,
            sortCreatedDate,
            sortModifiedDate,
            action,
            page,
            size);
        return Ok(result);
    }

    /// <summary>
    /// Create course 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = $"{Constant.AdminRole}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> CreateCourseAsync([FromBody] CreateCourseDto dto)
    {
        var result = await _courseService.CreateCourseAsync(dto);
        return Created(nameof(CreateCourseDto), result);
    }

    /// <summary>
    /// Update course. Teacher can save course as draft or post course base on the value of action parameter:
    /// "Save": save as draft; "Post" create post request. Can not update after create post request
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole}")]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> UpdateCourseAsync([FromRoute] int id, [FromBody] UpdateCourseDto dto,
        [FromQuery] string? action)
    {
        var result = await _courseService.UpdateCourseAsync(id, dto, action);
        return Ok(result);
    }

    /// <summary>
    /// Update course picture
    /// </summary>
    /// <param name="id"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole}")]
    [HttpPatch("{id:int}/picture")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    public async Task<ActionResult<CourseDto>> UpdateCoursePictureAsync([FromRoute] int id, [FromForm] IFormFile file)
    {
        var result = await _courseService.UpdateCoursePictureAsync(id, file);
        return Ok(result);
    }

    /// <summary>
    /// Admin or staff approve pending course
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("{id:int}/approve")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.StaffRole}")]
    public async Task<ActionResult> ApproveCourseAsync([FromRoute] int id, [FromBody] AcceptCourseDto dto)
    {
        await _courseService.ApproveCourseAsync(id, dto);
        return Ok();
    }

    /// <summary>
    /// Admin or staff approve pending course
    /// </summary>
    /// <param name="id"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    [HttpPatch("{id:int}/deny")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.StaffRole}")]
    public async Task<ActionResult> DenyCourseAsync([FromRoute] int id, [FromQuery] string? reason)
    {
        await _courseService.DenyCourseAsync(id, reason);
        return Ok();
    }

    /// <summary>
    /// Get Section component number of section in course. Ex a section has at most 5 videos, 3 documents,...
    /// </summary>
    /// <returns></returns>
    [HttpGet("sectionComponentNumber")]
    public async Task<ActionResult<List<SectionComponentNumberDto>>> GetSectionComponentNumberAsync()
    {
        return Ok(await _courseService.GetSectionComponentNumberAsync());
    }

    /// <summary>
    ///  Update Section component number of section in course.
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole}")]
    [HttpPut("sectionComponentNumber")]
    public async Task<ActionResult<List<SectionComponentNumberDto>>> UpdateSectionComponentNumberAsync(
        [FromBody] List<UpdateSectionComponentNumberDto> dtos)
    {
        var result = await _courseService.UpdateSectionComponentNumberAsync(dtos);
        return Ok(result);
    }

    /// <summary>
    /// Get thông tin course hiển thị ra trong màn hình coursepayment
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("payment/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseOrderDto>> GetCoursePaymentAsync(int id)
    {
        var result = await _courseService.GetCoursePaymentAsync(id);
        return Ok(result);
    }
}