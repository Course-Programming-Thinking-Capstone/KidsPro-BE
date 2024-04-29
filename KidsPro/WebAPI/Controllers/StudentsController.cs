using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Student;
using Application.Dtos.Response.StudentProgress;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/students")]
public class StudentsController : Controller
{
    private IStudentService _studentService;
    private IAuthenticationService _authentication;
    private IProgressService _progress;

    public StudentsController(IStudentService studentService, IAuthenticationService authentication,
        IProgressService progress)
    {
        _studentService = studentService;
        _authentication = authentication;
        _progress = progress;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentResponse>> GetStudents([FromQuery] int? classId)
    {
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();
        if (classId > 0)
            return Ok(await _studentService.GetStudentsAsync(classId ?? 0));
        return Ok(await _studentService.GetStudentsAsync());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"> </param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole},{Constant.StaffRole},{Constant.AdminRole}")]
    [HttpGet("detail/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDetailResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentDetailResponse>> GetStudentDetail(int id)
    {
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();

        var result = await _studentService.GetDetailStudentAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto">Gender is enum: "Male: 1, Female: 2".</param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole},{Constant.StudentRole}")]
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<IActionResult> UpdateStudentInformation(StudentUpdateRequest dto)
    {
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();

        await _studentService.UpdateStudentAsync(dto);
        return Ok("Update Student Information Success!");
    }

    /// <summary>
    /// Get student's course, include course ratio
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.StaffRole},{Constant.StudentRole}")]
    [HttpGet("progress/courses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<List<SectionProgressResponse>>> GetStudentCourseAsync()
    {
        var result = await _progress.GetStudentCoursesProgressAsync();

        return result != null ? Ok(result) : NotFound("Students do not have courses");
    }

    /// <summary>
    /// Get student's section progress 
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("progress/course/lessons")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<SectionProgressResponse>> GetSectionProgress(int studentId, int courseId)
    {
        var result = await _progress.GetCourseProgressAsync(studentId, courseId);
        return result != null ? Ok(result) : NotFound("The student does not has course");
    }
}