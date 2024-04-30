using Application.Configurations;
using Application.Dtos.Request.Teacher;
using Application.Interfaces.IServices;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/teachers")]
public class TeacherController : ControllerBase
{
    private ITeacherService _teacher;

    public TeacherController(ITeacherService teacher)
    {
        _teacher = teacher;
    }

    /// <summary>
    /// Teacher edit profile
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.TeacherRole}")]
    [HttpPut("profile")]
    public async Task<ActionResult<string>> UpdateProfileAsync(ProfileRequest dto)
    {
        await _teacher.TeacherEditProfile(dto, null, null, EditTeacherType.Profile);
        return Ok(new
        {
            Message = "Successfully update teacher profile"
        });
    }

    /// <summary>
    /// Teacher edit social profile
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.TeacherRole}")]
    [HttpPut("social")]
    public async Task<ActionResult<string>> UpdateSocialProfileAsync(SocialProfileRequest dto)
    {
        await _teacher.TeacherEditProfile(null, dto, null, EditTeacherType.SocialProfile);
        return Ok(new
        {
            Message = "Successfully update teacher social profile"
        });
    }

    /// <summary>
    /// Teacher edit certificate
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.TeacherRole}")]
    [HttpPut("certificate")]
    public async Task<ActionResult<string>> UpdateCertificateAsync(List<CertificateRequest> dto)
    {
        await _teacher.TeacherEditProfile(null, null, dto, EditTeacherType.Cerificate);
        return Ok(new
        {
            Message = "Successfully update teacher certificates"
        });
    }
}