using Application.Dtos.Response.Account;

namespace Application.Dtos.Response.Student;

public class StudentDto : AccountDto
{
    public int? Age { get; set; }
}