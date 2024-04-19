using Domain.Enums;

namespace Application.Dtos.Response.Class.TeacherSchedule;

public class TeacherClass
{
    public int ClassId  { get; set; }
    public string? ClassName { get; set; }
    public int? Slot { get; set; }
    public TimeSpan? Open { get; set; }
    public TimeSpan? Close { get; set; }
    public IEnumerable<DayStatus>? StudyDays { get; set; }= new List<DayStatus>();

}