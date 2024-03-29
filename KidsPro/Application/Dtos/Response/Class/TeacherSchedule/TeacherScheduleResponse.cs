namespace Application.Dtos.Response;

public class TeacherScheduleResponse
{
    public int TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public List<TeacherClass>? Schedules { get; set; }= new List<TeacherClass>();
}