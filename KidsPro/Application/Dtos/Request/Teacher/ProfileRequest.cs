using Application.Validations;
using Domain.Enums;

namespace Application.Dtos.Request.Teacher;

public class ProfileRequest
{
    [NameValidation]
    public string TeacherName { get; set; } = string.Empty;
    [AdultDayOfBirthValidation]
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    [PhoneValidation]
    public string? PhoneNumber { get; set; }
    public string? PersonalInformation { get; set; }
    
}