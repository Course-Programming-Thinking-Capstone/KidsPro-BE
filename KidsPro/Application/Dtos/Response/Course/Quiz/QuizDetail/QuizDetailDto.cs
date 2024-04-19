namespace Application.Dtos.Response.Course.Quiz.QuizDetail;

public class QuizDetailDto
{
    public int Id { get; set; }

    public int TotalQuestion { get; set; }

    public decimal TotalScore { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? Duration { get; set; }

    public int? NumberOfAttempt { get; set; }

    public List<QuestionDetailDto> Questions { get; set; } = new List<QuestionDetailDto>();
}