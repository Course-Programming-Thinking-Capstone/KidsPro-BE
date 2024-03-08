using Domain.Entities.Generic;

namespace Domain.Entities;

public class StudentOption : BaseEntity
{
    public int QuestionId { get; set; }

    public int OptionId { get; set; }

    public int QuestionOrder { get; set; }

    public virtual StudentQuiz StudentQuiz { get; set; } = null!;
    public int StudentQuizId { get; set; }
}