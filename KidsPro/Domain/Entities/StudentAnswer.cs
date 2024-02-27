using Domain.Entities.Generic;

namespace Domain.Entities;

public class StudentAnswer : BaseEntity
{
    public int QuestionId { get; set; }

    public int AnswerId { get; set; }

    public int QuestionOrder { get; set; }

    public virtual StudentQuiz StudentQuiz { get; set; } = null!;
    public int StudentQuizId { get; set; }
}