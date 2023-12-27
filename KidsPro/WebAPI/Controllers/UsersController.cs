using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;
using AutoMapper;
using Application.Dtos.Response.User;
using Application.Interfaces.IServices;

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
        //[HttpPost("roles")]
        //public async Task<IActionResult> AddRole(Role role)
        //{
        //    var result = await _roleService.AddRole(role);
        //    if (result) return Ok();
        //    return BadRequest();
        //}
        /// <summary>
        /// Get all roles in sever
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles= _roleService.GetRoles(); 
            if(roles.Any()) return Ok(roles);
            return NotFound();
        }

        //[HttpPut("roles")]
        //public async Task<IActionResult> UpdateRole(Role role)
        //{
        //    var result = await _roleService.UpdateRole(role);
        //    if (result) return Ok();
        //    return BadRequest();
        //}

        /// <summary>
        /// Get all users by role number
        /// </summary>
        /// <param name="role">1.Staff, 2.Parents, 3. Teachers</param>
        /// <returns></returns>
        [HttpGet]
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
        

    }
}