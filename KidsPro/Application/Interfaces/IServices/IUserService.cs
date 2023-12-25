using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.User;
using Domain.Entities;

namespace Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<LoginUserDto> LoginAsync(string phonenumber, string password);
        Task<User> GetUserById(Guid id);
        (bool, string, string?) ReissueToken(string accessToken, string refeshToken, User user);

        Task<LoginUserDto> RegisterAsync(RegisterDto request);
    }
}