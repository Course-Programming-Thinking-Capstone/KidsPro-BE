namespace Application.Dtos.Response.StudentProgress;

public class SectionProgressResponse
{
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? CourseName { get; set; }
    public List<ProgressResponse> Progress { get; set; } = new List<ProgressResponse>();
}