namespace Application.Dtos.Response.Course.Quiz;

public class QuestionDto
{
    public int QuestionId { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = null!;
    public decimal Score { get; set; }

    public virtual ICollection<OptionDto>? Options { get; set; }
}