namespace Application.Dtos.Response.Course.Quiz;

public class QuizSubmitResponse
{
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public List<QuizDto> Quizs { get; set; } = new List<QuizDto>();
}
