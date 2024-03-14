using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Paging;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
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
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagingResponse<FilterCourseDto>))]
    public async Task<ActionResult<PagingResponse<FilterCourseDto>>> FilterCourseAsync(
        [FromQuery] string? name,
        [FromQuery] CourseStatus? status,
        [FromQuery] string? sortName,
        [FromQuery] int? page,
        [FromQuery] int? size)
    {
        var result = await _courseService.FilterCourseAsync(name, status, sortName, page, size);
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
    [HttpPut("{id:int}")]
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
    /// <returns></returns>
    [HttpPatch("{id:int}/approve")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.StaffRole}")]
    public async Task<ActionResult> ApproveCourseAsync([FromRoute] int id)
    {
        await _courseService.ApproveCourseAsync(id);
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

    // [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    // [HttpPatch("{courseId:int}/section/order")]
    // public async Task<ActionResult<SectionDto>> UpdateSectionOrderAsync([FromRoute] int courseId,
    //     [FromBody] List<UpdateSectionOrderDto> dto)
    // {
    //     var result = await _courseService.UpdateSectionOrderAsync(courseId, dto);
    //     return Ok(result);
    // }

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

    // [Authorize(Roles = $"{Constant.AdminRole}")]
    // [HttpDelete("section/{sectionId:int}")]
    // public async Task<ActionResult> RemoveSectionAsync([FromRoute] int sectionId)
    // {
    //     await _courseService.RemoveSectionAsync(sectionId);
    //     return Ok();
    // }

    // [HttpPatch("section/video/{videoId:int}")]
    // [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    // public async Task<ActionResult<LessonDto>> UpdateVideoAsync([FromRoute] int videoId, [FromBody] UpdateVideoDto dto)
    // {
    //     var result = await _courseService.UpdateVideoAsync(videoId, dto);
    //     return Ok(result);
    // }

    // [HttpPatch("section/document/{documentId:int}")]
    // [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    // public async Task<ActionResult<LessonDto>> UpdateDocumentAsync([FromRoute] int documentId,
    //     [FromBody] UpdateDocumentDto dto)
    // {
    //     var result = await _courseService.UpdateDocumentAsync(documentId, dto);
    //     return Ok(result);
    // }

    // [HttpPatch("section/lesson/order")]
    // [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    // public async Task<ActionResult<ICollection<LessonDto>>> UpdateLessonOrderAsync(
    //     [FromBody] List<UpdateLessonOrderDto> dtos)
    // {
    //     var result = await _courseService.UpdateLessonOrderAsync(dtos);
    //     return Ok(result);
    // }
}