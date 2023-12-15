using Application.Configurations;
using Domain.Entities;

namespace Application.Services;

public interface IAuthenticationService
{

    string CreateAccessToken(User user);
    
    string CreateRefreshToken (User user);

    int GetCurrentUserId();

    void GetCurrentUserInformation(out int userId, out string role);

}