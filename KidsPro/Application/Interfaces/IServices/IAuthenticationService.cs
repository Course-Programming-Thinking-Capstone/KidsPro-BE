using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IAuthenticationService
{

    string CreateAccessToken(User user);
    
    string CreateRefreshToken (User user);

    Guid GetCurrentUserId();

    void GetCurrentUserInformation(out int userId, out string role);
    (bool, string, string?) ReissueToken(string accessToken, string refeshToken, User user);

}