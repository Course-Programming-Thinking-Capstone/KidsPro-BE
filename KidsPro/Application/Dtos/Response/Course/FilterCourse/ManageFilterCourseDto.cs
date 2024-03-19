namespace Application.Dtos.Response.Course.FilterCourse;

public class ManageFilterCourseDto:FilterCourseDto
{
    public string Status { get; set; } = string.Empty;

    public string? CreatedDate { get; set; }
}