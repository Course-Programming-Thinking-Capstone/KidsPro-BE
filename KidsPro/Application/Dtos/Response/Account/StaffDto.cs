namespace Application.Dtos.Response.Account;

public class StaffDto : AccountDto
{
    public string? Biography { get; set; }

    public string? ProfilePicture { get; set; }

    public string? PhoneNumber { get; set; }
}