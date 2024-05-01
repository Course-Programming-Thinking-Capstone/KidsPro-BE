using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Account.Parent;
using Application.Dtos.Response.Teacher;
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
    private IAuthenticationService _authentication;
    private ITeacherService _teacher;

    public ParentsController(IParentsService parent, IAuthenticationService authentication, ITeacherService teacher)
    {
        _parent = parent;
        _authentication = authentication;
        _teacher = teacher;
    }

    /// <summary>
    /// Parent add new child
    /// </summary>
    /// <param name="request">Gender is enum: "Male: 1, Female: 2".</param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.ParentRole}")]
    [HttpPost("student")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginAccountDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<LoginAccountDto>> AddStudent(StudentAddRequest request)
    {
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();
        
        var result = await _parent.AddStudentAsync(request);
        return Ok(result);
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
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();
        
        var result=await _parent.GetParentEmail();
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
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();
        
        var result=await _parent.GetListVoucherAsync(status);
        return Ok(result);
    }
    /// <summary>
    /// Parent get all teachers list
    /// </summary>
    /// <returns></returns>
    [HttpGet("teachers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<List<TeacherResponse>>> GetTeachersAsync()
    {
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();
        
        var result=await _teacher.GetTeachers();
        return Ok(result);
    }
    /// <summary>
    /// Parent get teacher detail information
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("teacher/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<AccountDto>> GetTeachersDetailAsync(int id)
    {
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();
        
        var result=await _teacher.GetTeacherDetail(id);
        return Ok(result);
    }
}
