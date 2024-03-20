using Application.Configurations;
using Application.Dtos.Request.Account.Student;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/staffs")]
public class StaffsController : ControllerBase
{
    private IStaffService _staff;

    public StaffsController(IStaffService staff)
    {
        _staff = staff;
    }
    
    /// <summary>
    /// Staff Create Student Account
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    //[Authorize(Roles = $"{Constant.StaffRole}")]
    [HttpPost("student/account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetail))]
    public async Task<IActionResult> CreaetAccountAsync(StudentCreateAccountRequest dto)
    {
        await _staff.CreateAccountStudentAsync(dto);
        return Ok();
    }
    
}