using Application.Configurations;
using Application.Dtos.Request.Student;
using Application.Dtos.Response.Account;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/parents")]
public class ParentsController : Controller
{
    IParentsService _parent;

    public ParentsController(IParentsService parent)
    {
        _parent = parent;
    }

    /// <summary>
    /// Phụ huynh thêm trẻ vào hệ thống
    /// </summary>
    /// <param name="request">Gender is enum: "Male: 1, Female: 2".</param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginAccountDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LoginAccountDto>> AddStudent(StudentAddDto request)
    {
        var result = await _parent.AddStudent(request);
        return Ok(result);
    }

    /// <summary>
    /// Phụ huynh Lấy danh sách student hiển thị by ParentId
    /// </summary>
    /// <param name="id">Parent Id</param>
    /// <returns></returns>
   // [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpGet("stduents/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentDto>> GetStudentsByParentId(int id)
    {
        var result = await _parent.GetStudents(id);
        return Ok(result);
    }

    /// <summary>
    /// Phụ huynh xem thông tin chi tiết của 1 trẻ
    /// </summary>
    /// <param name="id"> </param>
    /// <returns></returns>
    // [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpGet("stduent-detail/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDetailDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentDetailDto>> GetStudentDetail(int id)
    {
        var result = await _parent.GetDetailStudent(id);
        return Ok(result);
    }
    /// <summary>
    /// Update thông tin của trẻ
    /// </summary>
    /// <param name="dto">Gender is enum: "Male: 1, Female: 2".</param>
    /// <returns></returns>
    [HttpPut("update-student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<IActionResult> UpdateStudentInformation(StudentUpdateDto dto)
    {
        await _parent.UpdateStudent(dto);
        return Ok("Update Student Information Success!");
    }
}
