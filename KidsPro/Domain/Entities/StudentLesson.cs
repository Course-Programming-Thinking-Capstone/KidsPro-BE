using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[PrimaryKey(nameof(StudentId), nameof(LessonId))]
public class StudentLesson
{
    public virtual Student Student { get; set; } = null!;
    public int StudentId { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;
    public int LessonId { get; set; }

    public bool IsCompleted { get; set; }
}