namespace Application.Dtos.Request.Course.Lesson;

public record UpdateDocumentDto : UpdateLessonDto
{
    public string? Content { get; init; }
};