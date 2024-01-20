using Application.Dtos.Response.Course.Section;

namespace Application.Dtos.Response.Course;

public class CourseDetailDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Prerequisite { get; set; }

    public int? FromAge { get; set; }

    public int? ToAge { get; set; }

    public string? PictureUrl { get; set; }

    public DateTime? OpenDate { get; set; }

    public DateTime? PostedDate { get; set; }

    public DateTime? StartSaleDate { get; set; }

    public DateTime? EndSaleDate { get; set; }

    public decimal? Price { get; set; }

    public decimal? DiscountPrice { get; set; }

    public int TotalLesson { get; set; }

    public string Status { get; set; } = null!;

    public string CreatedDate { get; set; } = null!;

    public string ModifiedDate { get; set; } = null!;

    public int CreatedById { get; set; }

    public string CreatedBy { get; set; } = null!;

    public int ModifiedById { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public ICollection<SectionDto>? Sections { get; set; }

    public ICollection<CourseResourceDto>? Resources { get; set; }
}