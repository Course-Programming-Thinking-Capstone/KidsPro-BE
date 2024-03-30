using Domain.Enums;

namespace Application.Dtos.Response.StudentSchedule;

public class StudentClassResponse
{
    public string? Image { get; set; }
    public string? StudentName { get; set; }
    public string? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
}