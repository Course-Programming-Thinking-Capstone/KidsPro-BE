namespace Application.Dtos.Response.Course;

public class CommonCourseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? PictureUrl { get; set; }

    public int? FromAge { get; set; }

    public int? ToAge { get; set; }

    public string? OpenDate { get; set; }

    public string? StartSaleDate { get; set; }

    public string? EndSaleDate { get; set; }

    public decimal? Price { get; set; }

    public decimal? DiscountPrice { get; set; }
}