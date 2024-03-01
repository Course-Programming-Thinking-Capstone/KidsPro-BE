namespace Application.Dtos.Response.Account;

public class StudentGameLoginDto
{
    public int UserId { get; set; }

    public string DisplayName { get; set; } = null!;

    public int UserCoin { get; set; }

    public int UserGem { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }
}