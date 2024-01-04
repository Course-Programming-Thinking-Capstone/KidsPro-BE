using Application.Dtos.Request.Course;
using Application.Dtos.Response.Course;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/v1/courses")]
[ApiController]
public class CoursesController : Controller
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    /// <summary>
    /// Admin hoặc teacher có thể tạo khóa học trên hệ thống. Khóa học được tạo sẽ có status draft, cần phải 
    /// tạo yêu cầu duyệt 
    /// để đưa khóa học lên trên website.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> CreateAsync([FromBody] CreateCourseDto request)
    {
        var result = await _courseService.CreateAsync(request);
        return Created(nameof(CreateAsync), result);
    }

    /// <summary>
    /// Admin hoặc người tạo khóa học có thể thay đổi thông tin cơ bản của khóa học.
    /// Chỉ có thể thay đổi thông tin khi khóa học có status draft
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> UpdateAsync([FromRoute] int id, [FromBody] UpdateCourseDto request)
    {
        return Ok(await _courseService.UpdateAsync(id, request));
    }

    /// <summary>
    /// Upload ảnh bìa cho khóa học 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPatch("{id:int}/picture")]
    [Authorize(Roles = "Admin,Teacher,Staff")]
    [Consumes("image/jpeg", "image/png", "multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<CourseDto>> UpdatePictureAsync([FromRoute] int id,
        [FromForm(Name = "file")] IFormFile file)
    {
        return Ok(await _courseService.UpdatePictureAsync(id, file));
    }

    /// <summary>
    ///  Delete unused course. Only admin or owner can delete course by Id and can only delete draft course
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Teacher,Staff")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetail))]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        await _courseService.DeleteAsync(id);
        return NoContent();
    }
}