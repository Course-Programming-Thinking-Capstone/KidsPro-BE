namespace Application.Dtos.Response.Account;

public class TeacherDto : AccountDto
{
    public string? Field { get; set; }

    public string? PersonalInformation { get; set; }

    public string? Biography { get; set; }

    public string? ProfilePicture { get; set; }

    public string? PhoneNumber { get; set; }
}