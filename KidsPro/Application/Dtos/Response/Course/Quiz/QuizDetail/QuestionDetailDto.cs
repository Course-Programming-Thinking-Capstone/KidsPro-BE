namespace Application.Dtos.Response.Course.Quiz.QuizDetail;

public class QuestionDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Score { get; set; }

    public List<OptionDetailDto> Options { get; set; } = new List<OptionDetailDto>();
}