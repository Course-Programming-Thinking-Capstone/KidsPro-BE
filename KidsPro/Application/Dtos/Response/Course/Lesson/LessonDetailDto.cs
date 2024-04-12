namespace Application.Dtos.Response.Course.Lesson;

public class LessonDetailDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Order { get; set; }

    public string? Content { get; set; }

    public string? ResourceUrl { get; set; }

    public int? Duration { get; set; }

    public string Type { get; set; } = null!;
}