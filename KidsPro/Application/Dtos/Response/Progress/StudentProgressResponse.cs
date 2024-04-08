namespace Application.Dtos.Response.StudentProgress;

public class StudentProgressResponse
{
    public int ProgressId { get; set; }
    public int SectionId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string? EnrolledDate { get; set; }
    public string? CompletedDate { get; set; }
}