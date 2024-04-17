namespace Application.Dtos.Response.Course.Study;

public class CommonStudyQuizDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    
    public int TotalQuestion { get; set; }
    
    public int? Duration { get; set; }
    
}