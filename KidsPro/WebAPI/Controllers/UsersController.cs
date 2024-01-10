using Application.Interfaces.IServices;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;
using AutoMapper;
using Application.Dtos.Response.User;
using Microsoft.AspNetCore.Authorization;
using Application.ErrorHandlers;
using Application.Configurations;

namespace WebAPI.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IRoleService _roleService;
        IMapper _map;

        public UsersController(IUserService userService, IRoleService roleService, IMapper map)
        {
            _userService = userService;
            _roleService = roleService;
            _map = map;
        }

        /// <summary>
        /// Get all roles in sever
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles =Constant.AdminOrStaffRole)]
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles= _roleService.GetRoles(); 
            if(roles.Any()) return Ok(roles);
            return NotFound();
        }

        /// <summary>
        /// Get all users trong database theo role number
        /// </summary>
        /// <param name="role">2.Staff, 3. Teacher, 4.Parent</param>
        /// <returns></returns>
        [Authorize(Roles =Constant.AdminRole)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type = typeof(ErrorDetail))]
        public async Task<IActionResult> GetAllUsers(RoleType role)
        {
            var users = await _userService.GetAllUsersByRole((int)role);
            var result= _map.Map<List<UserResponseDto>>(users);
            if(result.Any())
            {
                return Ok(result);
            }
            return NotFound();

        }
       /// <summary>
       /// Switch user status, active or deactive
       /// </summary>
       /// <param name="id"></param>
       /// <param name="status">1. Active, 2. Deactive</param>
       /// <returns></returns>
        [Authorize(Roles =Constant.AdminRole)]
        [HttpPatch("switch/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDetail))]
        [ProducesResponseType(StatusCodes.Status409Conflict,Type = typeof(ErrorDetail))]
        public async Task<IActionResult> SwitchStatusUser([FromRoute] int id,UserStatus status)
        {
            var result = false;
            switch ((int)status)
            {
                case 1:
                   result= await _userService.SwitchStatusUser(id,1);
                   break;
                case 2:
                    result = await _userService.SwitchStatusUser(id, 2);
                    break;
            }
            return Ok(result);

        }
        

    }
}