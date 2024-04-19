namespace Application.Dtos.Response.Class.TeacherClass;

public class TeacherClassStudentDto
{
    public int AccountId { get; set; }

    public string FullName { get; set; } = null!;
    
    public string? PictureUrl { get; set; } 
    
    public int? Age { get; set; }

    public string? Gender { get; set; } = null!;
}