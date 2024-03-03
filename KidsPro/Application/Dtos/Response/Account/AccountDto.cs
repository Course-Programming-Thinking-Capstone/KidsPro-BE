namespace Application.Dtos.Response.Account;

public class AccountDto
{
    public int Id { get; set; }

    public byte[] Version { get; set; } = null!;

    public string? Email { get; set; }

    public string FullName { get; set; } = null!;

    public string? PictureUrl { get; set; }

    public string? Gender { get; set; }

    public string? DateOfBirth { get; set; }

    public string Status { get; set; } = null!;

    public string CreatedDate { get; set; } = null!;

    public string Role { get; set; } = null!;
}