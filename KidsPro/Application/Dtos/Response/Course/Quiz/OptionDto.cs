namespace Application.Dtos.Response.Course.Quiz;

public class OptionDto
{
    public int Order { get; set; }
    public string Content { get; set; } = null!;
    public string? AnswerExplain { get; set; }
    public bool IsCorrect { get; set; }
}