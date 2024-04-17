namespace Application.Dtos.Response.Course.Study;

public class StudyCourseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? PictureUrl { get; set; }
    
    public bool IsFree { get; set; }
    
    public int TotalSection { get; set; }
    
    public int TotalVideo { get; set; }
    
    public int TotalDocument { get; set; }
    
    public int TotalQuiz { get; set; }

    public ICollection<CommonStudySectionDto> Sections { get; set; } = new List<CommonStudySectionDto>();

}