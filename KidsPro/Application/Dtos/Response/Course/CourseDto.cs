namespace Application.Dtos.Response.Course;

public class CourseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Prerequisite { get; set; }

    public int? FromAge { get; set; }
    
    public int? ToAge { get; set; }

    public string? PictureUrl { get; set; }

    public string? OpenDate { get; set; }

    public string? StartSaleDate { get; set; }

    public string? EndSaleDate { get; set; }

    public decimal? Price { get; set; }

    public decimal? DiscountPrice { get; set; }

    public int TotalLesson { get; set; }

    public string Status { get; set; } = string.Empty;

    public string CreatedDate { get; set; } = string.Empty;

    public string ModifiedDate { get; set; } = string.Empty;

    public int CreatedById { get; set; }

    public string CreatedByName { get; set; } = string.Empty;

    public int ModifiedById { get; set; }

    public string ModifiedByName { get; set; } = string.Empty;

    public ICollection<CourseResourceDto>? Resources { get; set; } 

}