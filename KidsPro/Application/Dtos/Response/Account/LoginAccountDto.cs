namespace Application.Dtos.Response.Account;

public class LoginAccountDto
{
    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string PictureUrl { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? AccessToken { get; set; } 
    
    public string? RefreshToken { get; set; }
}