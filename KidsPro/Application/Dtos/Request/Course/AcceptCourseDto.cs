using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Course;

public record AcceptCourseDto
{
    public bool IsFree { get; set; }
    
    public bool IsAdminSetup { get; set; } 
    
    [Range(0, 100000000)]
    public decimal? Price { get; set; }
}