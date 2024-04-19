using Domain.Enums;

namespace Application.Dtos.Response.Class.TeacherClass;

public class TeacherClassDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;

    public int Duration { get; set; }

    public string Status { get; set; } = null!;

    public int? TotalSlot { get; set; }

    public int? TotalStudent { get; set; }

    public int CourseId { get; set; }
    public string CourseName { get; set; } = null!;

    public ICollection<TeacherClassStudentDto> Students { get; set; } = new List<TeacherClassStudentDto>();
}