namespace Application.Dtos.Response.Account;

public class LoginAccountDto
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string FullName { get; set; } = null!;
    public string? PictureUrl { get; set; }
    public string Role { get; set; } = null!;
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}