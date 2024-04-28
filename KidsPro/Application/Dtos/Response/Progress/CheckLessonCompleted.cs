namespace Application.Dtos.Response.StudentProgress;

public class CheckLessonCompleted
{
    public int LessonId { get; set; }
    public bool IsCompleted { get; set; }
}