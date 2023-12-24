using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
            var result = _userService.ReissueToken(accessToken, refeshToken, user);
            if (result.Item1)
                return Ok(result);
            return NotFound(result.Item2);
        }
    }
}