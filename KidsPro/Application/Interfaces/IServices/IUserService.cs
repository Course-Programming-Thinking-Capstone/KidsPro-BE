using Application.Dtos.Request.Authentication;
using Application.Dtos.Response.User;
using Domain.Entities;

namespace Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<LoginUserDto> LoginAsync(string phonenumber, string password);
        Task<User> GetUserById(Guid id);
        Task<(bool, string, string?)> ReissueToken(string accessToken, string refeshToken, int id);

        Task<LoginUserDto> RegisterAsync(RegisterDto request, int number);
        Task<List<User>> GetAllUsersByRole(int role);
        Task<bool> SwitchStatusUser(int id, int number);


    }
}