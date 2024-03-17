using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Account.Parent;
using Application.Dtos.Response.Account.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/parents")]
public class ParentsController : ControllerBase
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
   // [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpPost("student")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginAccountDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LoginAccountDto>> AddStudent(StudentAddRequestDto request)
    {
        var result = await _parent.AddStudentAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Phụ huynh Lấy danh sách student hiển thị by ParentId
    /// </summary>
    /// <param name="id">Parent Id</param>
    /// <returns></returns>
   // [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpGet("stduents/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentResponseDto>> GetStudentsByParentId(int id)
    {
        var result = await _parent.GetStudentsAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Phụ huynh xem thông tin chi tiết của 1 trẻ
    /// </summary>
    /// <param name="id"> </param>
    /// <returns></returns>
    // [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpGet("stduent-detail/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDetailResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentDetailResponseDto>> GetStudentDetail(int id)
    {
        var result = await _parent.GetDetailStudentAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Update thông tin của trẻ
    /// </summary>
    /// <param name="dto">Gender is enum: "Male: 1, Female: 2".</param>
    /// <returns></returns>
    [HttpPut("student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<IActionResult> UpdateStudentInformation(StudentUpdateRequestDto dto)
    {
        await _parent.UpdateStudentAsync(dto);
        return Ok("Update Student Information Success!");
    }

    /// <summary>
    /// Get email and zalo của parent
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpGet("email-zalo/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public ActionResult<ParentOrderResponseDto> GetEmailZalo(int id)
    {
        var result= _parent.GetEmailZalo(id);
        return Ok(result);
    }
}
