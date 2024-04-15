namespace Application.Dtos.Response.Course.Quiz;

public class QuizSubmitDto
{
    public decimal Score { get; set; }
    public int AttemptTurnNumber { get; set; }
    public int? Duration { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public bool IsPass { get; set; }
    public List<QuestionDto> QuestionDtos { get; set; } = new List<QuestionDto>();
}