namespace Application.Dtos.Request.Class;

public class StudentsAddRequest
{
    public List<int> StudentIds { get; set; } = new List<int>();
    public int ClassId { get; set; } 
}