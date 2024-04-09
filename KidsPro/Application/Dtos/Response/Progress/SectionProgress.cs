namespace Application.Dtos.Response.StudentProgress;

public class SectionProgress
{
    public int SectionId { get; set; }
    public string? SectionName { get; set; }
    public float Progress { get; set; }
}