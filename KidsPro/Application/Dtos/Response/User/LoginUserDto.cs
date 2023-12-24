using Domain.Enums;

namespace Application.Dtos.Response.User;

public class LoginUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? PictureUrl { get; set; }
    public string? Gender { get; set; }
    public string Role { get; set; } = null!;
}