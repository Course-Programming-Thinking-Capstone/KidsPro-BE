namespace Application.Dtos.Response.Course;

public class CourseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? PictureUrl { get; set; }

    public string? StartSaleDate { get; set; }

    public string? EndSaleDate { get; set; }

    public decimal? Price { get; set; }

    public decimal? DiscountPrice { get; set; }

    public int? TotalLesson { get; set; }

    public bool IsFree { get; set; }

    public ICollection<SectionDto>? Sections { get; set; }
}