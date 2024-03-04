namespace Application.Dtos.Response.Course;

public class SectionDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }
    
    public int CourseId { get; set; }
}