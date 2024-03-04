namespace Application.Dtos.Response.Course;

public class SectionDto
{
    public int Id { get; set; }

    public byte[] Version { get; set; } = null!;

    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }
}