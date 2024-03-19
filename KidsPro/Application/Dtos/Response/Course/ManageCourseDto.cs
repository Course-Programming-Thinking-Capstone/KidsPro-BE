namespace Application.Dtos.Response.Course;

public class ManageCourseDto : CourseDto
{
    
    public string Status { get; set; } = string.Empty;

    public string CreatedDate { get; set; } = string.Empty;

    public string ModifiedDate { get; set; } = string.Empty;

    public int CreatedById { get; set; }

    public string CreatedByName { get; set; } = string.Empty;

    public int? ModifiedById { get; set; }

    public string? ModifiedByName { get; set; }

    public int? ApprovedById { get; set; }
    public string? ApprovedByName { get; set; }
}