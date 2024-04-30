using Domain.Enums;

namespace Application.Dtos.Request.Teacher;

public class EditProfileRequest
{
    public string? TeacherName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Personal { get; set; }
    
}