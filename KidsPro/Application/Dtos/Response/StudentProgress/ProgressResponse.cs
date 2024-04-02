namespace Application.Dtos.Response.StudentProgress;

public class ProgressResponse
{
    public int SectionId { get; set; }
    public string? SectionName { get; set; }
    public float Progress { get; set; }
}