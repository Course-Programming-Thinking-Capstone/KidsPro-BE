﻿using Application.Configurations;
using Application.Dtos.Request.Class;
using Application.Dtos.Response;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/classes")]
public class ClassController : ControllerBase
{
    private IClassService _class;
    private IAuthenticationService _authentication;

    public ClassController(IClassService @class, IAuthenticationService authentication)
    {
        _class = @class;
        _authentication = authentication;
    }

    /// <summary>
    /// Create Class
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.StaffRole},{Constant.AdminRole}")]
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<ClassCreateResponse>> CreateClassAsync(ClassCreateRequest dto)
    {
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();

        var result = await _class.CreateClassAsync(dto);
        return Ok(result);
    }
    
    /// <summary>
    /// Add Schedule
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.StaffRole},{Constant.AdminRole}")]
    [HttpPost("schedules")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<ScheduleCreateResponse>> CreateScheduleAsync(ScheduleCreateRequest dto)
    {
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();

        var result = await _class.CreateScheduleAsync(dto);
        return Ok(result);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.StaffRole},{Constant.AdminRole}")]
    [HttpGet("teachers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
    public async Task<ActionResult<List<TeacherScheduleResponse>>> GetTeacherAsync()
    {
        //Check if the account is activated or not or inactive
        _authentication.CheckAccountStatus();

        var result = await _class.GetTeacherToClassAsync();
        return Ok(result);
    }
}