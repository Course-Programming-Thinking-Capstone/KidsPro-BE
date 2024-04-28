namespace Application.Dtos.Response.Course.Study;

public class StudyLessonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ResourceUrl { get; set; }
    public string? Content { get; set; }
    public int? Duration { get; set; }
    public bool IsFree { get; set; }
    public string Type { get; set; } = null!;
    public bool IsComplete { get; set; } 
}