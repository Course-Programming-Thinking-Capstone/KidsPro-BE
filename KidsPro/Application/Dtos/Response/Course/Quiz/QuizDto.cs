namespace Application.Dtos.Response.Course.Quiz;

public class QuizDto
{
    public int Id { get; set; }

    public int Order { get; set; }
    public int TotalQuestion { get; set; }

    public decimal TotalScore { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? Duration { get; set; }

    public int NumberOfQuestion { get; set; }

    public bool IsOrderRandom { get; set; }

    public int? NumberOfAttempt { get; set; }

    public string CreatedDate { get; set; } = null!;

    public string CreatedByName { get; set; } = null!;
    public int CreatedById { get; set; }

    public virtual ICollection<QuestionDto>? Questions { get; set; }
}