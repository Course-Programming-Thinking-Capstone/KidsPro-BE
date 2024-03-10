namespace Application.Dtos.Request.Course.Lesson;

public record CreateDocumentDto : CreateLessonDto
{
    public string? Content { get; set; }
}