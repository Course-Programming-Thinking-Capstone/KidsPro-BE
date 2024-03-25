using Domain.Enums;

namespace Application.Dtos.Response;

public class TeacherCouse
{
    public int CourseId  { get; set; }
    public string? CourseName { get; set; }
    public IEnumerable<DayStatus> StudyDays = new List<DayStatus>();
    public int Slot { get; set; }
    public TimeSpan Open { get; set; }
    public TimeSpan Close { get; set; }
}