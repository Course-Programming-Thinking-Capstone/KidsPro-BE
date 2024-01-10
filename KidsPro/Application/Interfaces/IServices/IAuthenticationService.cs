using Domain.Entities;

namespace Application.Interfaces.IServices;

public interface IAuthenticationService
{

    string CreateAccessToken(User user);
    
    string CreateRefreshToken (User user);

    int GetCurrentUserId();

    void GetCurrentUserInformation(out int userId, out string role);
    public Task<(bool, string, string?)> ReissueToken(string accessToken, string refeshToken, int id);

}