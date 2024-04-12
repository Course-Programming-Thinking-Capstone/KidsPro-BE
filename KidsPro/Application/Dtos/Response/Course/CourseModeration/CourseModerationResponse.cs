using Domain.Enums;

namespace Application.Dtos.Response.Course.CourseModeration;

public class CourseModerationResponse
{
    public int CourseId { get; set; }
    public string? ImageUrl { get; set; }
    public string? CourseName { get; set; }
    public string? TeacherName { get; set; }
    public int TotalLesson { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public CourseStatus Status { get; set; }
}