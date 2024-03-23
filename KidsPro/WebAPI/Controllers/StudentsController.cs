using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account.Student;
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

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentResponse>> GetStudents()
    {
        var result = await _studentService.GetStudentsAsync();
        return Ok(result);
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
        var result = await _studentService.GetDetailStudentAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto">Gender is enum: "Male: 1, Female: 2".</param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<IActionResult> UpdateStudentInformation(StudentUpdateRequest dto)
    {
        await _studentService.UpdateStudentAsync(dto);
        return Ok("Update Student Information Success!");
    }

}