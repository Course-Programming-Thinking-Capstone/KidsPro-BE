using Application.Services;
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

        /// <summary>
        /// Login to the system
        /// </summary>
        /// <param name="phonenumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(string phonenumber, string password)
        {
            var checkLogin=await _userService.LoginAsync(phonenumber, password);
            return Ok(checkLogin);
        }
    }
}
