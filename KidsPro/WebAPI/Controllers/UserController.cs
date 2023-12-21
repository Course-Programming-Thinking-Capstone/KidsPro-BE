using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(string phonenumber, string password)
        {
            var result = await _userService.LoginAsync(phonenumber, password);
            if (result.Item1)
                 return Ok(result);
            return Unauthorized(result.Item2);
        }
        /// <summary>
        /// Reissue Token Including Access & Refesh Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refeshToken"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("reissue")]
        public IActionResult ReissueToken(string accessToken, string refeshToken, User user)
        {
            var result =  _userService.ReissueToken(accessToken, refeshToken,user);
            if (result.Item1)
                return Ok(result);
            return NotFound(result.Item2);
        }
        
    }
}
