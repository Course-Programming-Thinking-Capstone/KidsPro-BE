using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Progress;

public class StudentProgressRequest
{
    [Required] public int SectionId { get; set; }
    [Required] public int CourseId { get; set; }
}