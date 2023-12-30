using Domain.Entities;

namespace Application.Interfaces.IServices;

public interface IAuthenticationService
{

    string CreateAccessToken(User user);
    
    string CreateRefreshToken (User user);

    int GetCurrentUserId();

    void GetCurrentUserInformation(out int userId, out string role);
    (bool, string, string?) ReissueToken(string accessToken, string refeshToken, User user);

}