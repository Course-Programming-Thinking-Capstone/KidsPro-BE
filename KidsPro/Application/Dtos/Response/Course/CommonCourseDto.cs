namespace Application.Dtos.Response.Course;

public class CommonCourseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? PictureUrl { get; set; }

    public string Status { get; set; } = string.Empty;

    public string CreatedDate { get; set; } = string.Empty;

    public int CreatedById { get; set; }

    public string CreatedByName { get; set; } = string.Empty;

}