namespace Application.Dtos.Response.StudentProgress;

public class SectionProgressResponse
{
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    public string? TeacherName { get; set; }
    public List<ProgressResponse> SectionProgress { get; set; } = new List<ProgressResponse>();
    public float CourseProgress { get; set; } = 0;
}