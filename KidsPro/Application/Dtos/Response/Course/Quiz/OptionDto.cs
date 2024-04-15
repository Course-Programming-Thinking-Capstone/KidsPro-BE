namespace Application.Dtos.Response.Course.Quiz;

public class OptionDto
{
    public int OptionId { get; set; }
    public int Order { get; set; }
    public string Content { get; set; } = null!;
    public string? AnswerExplain { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsStudentChoose { get; set; }
}