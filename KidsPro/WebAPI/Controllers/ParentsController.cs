using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Account.Parent;
using Application.Dtos.Response.Account.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Enums;
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
    [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpPost("student")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginAccountDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LoginAccountDto>> AddStudent(StudentAddRequest request)
    {
        var result = await _parent.AddStudentAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Phụ huynh Lấy danh sách student hiển thị by ParentId
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpGet("students")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentResponse>> GetStudentsByParentId()
    {
        var result = await _parent.GetStudentsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Phụ huynh xem thông tin chi tiết của 1 trẻ
    /// </summary>
    /// <param name="id"> </param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpGet("student-detail/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentDetailResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<StudentDetailResponse>> GetStudentDetail(int id)
    {
        var result = await _parent.GetDetailStudentAsync(id);
        return Ok(result);
    }
    /// <summary>
    /// Update thông tin của trẻ
    /// </summary>
    /// <param name="dto">Gender is enum: "Male: 1, Female: 2".</param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpPut("student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<IActionResult> UpdateStudentInformation(StudentUpdateRequest dto)
    {
        await _parent.UpdateStudentAsync(dto);
        return Ok("Update Student Information Success!");
    }

    /// <summary>
    /// Get email and zalo của parent
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpGet("contact")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<ParentOrderResponse>> GetEmailZalo()
    {
        var result=await _parent.GetEmailZalo();
        return Ok(result);
    }
    
    /// <summary>
    /// Get list voucher of parent
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpGet("vouchers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<ParentOrderResponse>> GetListVoucherAsync(VoucherStatus status)
    {
        var result=await _parent.GetListVoucherAsync(status);
        return Ok(result);
    }
}
