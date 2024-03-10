namespace Application.Dtos.Response.Course.Lesson;

public class LessonDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public int? Duration { get; set; }

    public bool IsFree { get; set; }
    public bool IsDelete { get; set; }

    public string Type { get; set; } = null!;
    
    public int SectionId { get; set; }
}