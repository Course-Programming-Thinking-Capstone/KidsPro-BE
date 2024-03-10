﻿using Application.Dtos.Request.Course;
using Application.Dtos.Request.Course.Lesson;
using Application.Dtos.Request.Course.Section;
using Application.Dtos.Response.Course;
using Application.Dtos.Response.Course.Lesson;
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
    [HttpGet("{id:int}")]
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
    [HttpPatch("{id:int}")]
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
    /// Create course section.
    /// id in route is course's Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    [HttpPost("{id:int}/section")]
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
    [HttpPatch("section/{sectionId:int}")]
    public async Task<ActionResult<SectionDto>> UpdateSectionAsync([FromRoute] int sectionId,
        [FromBody] UpdateSectionDto dto)
    {
        var result = await _courseService.UpdateSectionAsync(sectionId, dto);
        return Ok(result);
    }

    /// <summary>
    /// Update section order
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    [HttpPatch("{courseId:int}/section/order")]
    public async Task<ActionResult<SectionDto>> UpdateSectionOrderAsync([FromRoute] int courseId,
        [FromBody] List<UpdateSectionOrderDto> dto)
    {
        var result = await _courseService.UpdateSectionOrderAsync(courseId, dto);
        return Ok(result);
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
    /// remove course section
    /// </summary>
    /// <param name="sectionId"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.AdminRole}")]
    [HttpDelete("section/{sectionId:int}")]
    public async Task<ActionResult> RemoveSectionAsync([FromRoute] int sectionId)
    {
        await _courseService.RemoveSectionAsync(sectionId);
        return Ok();
    }

    /// <summary>
    /// Add video to course section
    /// </summary>
    /// <param name="sectionId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("section/{sectionId:int}/video")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    public async Task<ActionResult<LessonDto>> AddVideoAsync([FromRoute] int sectionId, [FromBody] CreateVideoDto dto)
    {
        var result = await _courseService.AddVideoAsync(sectionId, dto);
        return Ok(result);
    }

    /// <summary>
    /// Update video information 
    /// </summary>
    /// <param name="videoId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("section/video/{videoId:int}")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    public async Task<ActionResult<LessonDto>> UpdateVideoAsync([FromRoute] int videoId, [FromBody] UpdateVideoDto dto)
    {
        var result = await _courseService.UpdateVideoAsync(videoId, dto);
        return Ok(result);
    }

    /// <summary>
    /// Add document to section
    /// </summary>
    /// <param name="sectionId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("section/{sectionId:int}/document")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    public async Task<ActionResult<LessonDto>> AddDocumentAsync([FromRoute] int sectionId,
        [FromBody] CreateDocumentDto dto)
    {
        var result = await _courseService.AddDocumentAsync(sectionId, dto);
        return Ok(result);
    }

    /// <summary>
    /// Update document information
    /// </summary>
    /// <param name="documentId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPatch("section/document/{documentId:int}")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    public async Task<ActionResult<LessonDto>> UpdateDocumentAsync([FromRoute] int documentId,
        [FromBody] UpdateDocumentDto dto)
    {
        var result = await _courseService.UpdateDocumentAsync(documentId, dto);
        return Ok(result);
    }

    /// <summary>
    /// Update lesson order
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    [HttpPatch("section/lesson/order")]
    [Authorize(Roles = $"{Constant.AdminRole},{Constant.TeacherRole},{Constant.StaffRole}")]
    public async Task<ActionResult<ICollection<LessonDto>>> UpdateLessonOrderAsync(
        [FromBody] List<UpdateLessonOrderDto> dtos)
    {
        var result = await _courseService.UpdateLessonOrderAsync(dtos);
        return Ok(result);
    }
}