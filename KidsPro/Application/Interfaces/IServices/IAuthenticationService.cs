using Domain.Entities;

namespace Application.Interfaces.IServices;

public interface IAuthenticationService
{
    string CreateAccessToken(Account user);

    string CreateRefreshToken(Account user);

    int GetCurrentUserId();

    void GetCurrentUserInformation(out int accountId, out string role);

    void CheckAccountStatus();
    // public Task<(bool, string, string?)> ReissueToken(string accessToken, string refeshToken, int id);
}