using Domain.Entities;

namespace Application.Dtos.Response.Course;

public class CommonCourseDto : CourseDto
{
    public int? TotalVideo { get; set; }
    public int? TotalDocument { get; set; }
    public int? TotalQuiz { get; set; }
}