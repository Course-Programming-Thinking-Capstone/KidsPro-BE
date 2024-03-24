using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.Dtos.Response.Account;
using Application.Dtos.Response.Account.Parent;
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

    public ParentsController(IParentsService parent, IAuthenticationService authentication)
    {
        _parent = parent;
        _authentication = authentication;
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
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();
        
        var result=await _parent.GetListVoucherAsync(status);
        return Ok(result);
    }
}
