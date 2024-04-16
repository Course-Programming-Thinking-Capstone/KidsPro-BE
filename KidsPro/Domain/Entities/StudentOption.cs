using Domain.Entities.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;
public class StudentOption : BaseEntity
{
    public int OptionId { get; set; }

    public int QuestionOrder { get; set; }

    public virtual StudentQuiz StudentQuiz { get; set; } = null!;
    public int StudentQuizId { get; set; }

    public virtual Question Question { get; set; } = null!;
    public virtual int QuestionId { get; set; }
}