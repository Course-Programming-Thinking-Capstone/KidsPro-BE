namespace Application.Dtos.Response.Course.FilterCourse;

public class FilterCourseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? PictureUrl { get; set; }
    
    public decimal? Price { get; set; }
    
    public bool IsFree { get; set; }
    
    
}