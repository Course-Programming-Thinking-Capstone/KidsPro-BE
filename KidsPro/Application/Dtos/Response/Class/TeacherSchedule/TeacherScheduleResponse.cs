namespace Application.Dtos.Response;

public class TeacherScheduleResponse
{
    public int TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public List<TeacherCouse>? Schedules = new List<TeacherCouse>();
}