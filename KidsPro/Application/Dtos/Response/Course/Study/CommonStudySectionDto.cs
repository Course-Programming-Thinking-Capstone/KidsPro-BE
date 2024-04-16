namespace Application.Dtos.Response.Course.Study;

public class CommonStudySectionDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int SectionTime { get; set; }

    public bool? IsBlock { get; set; } // for learning only

    public ICollection<CommonStudyLessonDto> Lessons { get; set; } = new List<CommonStudyLessonDto>();

    public ICollection<CommonStudyQuizDto> Quizzes { get; set; } = new List<CommonStudyQuizDto>();
}