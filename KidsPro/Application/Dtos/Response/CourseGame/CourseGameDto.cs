namespace Application.Dtos.Response.CourseGame;

public class CourseGameDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Status { get; set; } = null!;
}