using System.ComponentModel.DataAnnotations;
namespace Application.Dtos.Request.Class;

public class ClassCreateRequest
{
    
    [StringLength(10)] public string ClassCode { get; set; } = string.Empty;
    [DataType(DataType.Date)]
    public DateTime OpenDay { get; set; }
    [DataType(DataType.Date)]
    public DateTime CloseDay { get; set; }
    public int CourseId { get; set; }
}