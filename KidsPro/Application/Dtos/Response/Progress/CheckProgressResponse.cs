namespace Application.Dtos.Response.StudentProgress;

public class CheckProgressResponse
{
    public int SectionId { get; set; }
    public bool IsCheck { get; set; }
    public List<CheckLessonCompleted> Lesson { get; set; } = new List<CheckLessonCompleted>();
}