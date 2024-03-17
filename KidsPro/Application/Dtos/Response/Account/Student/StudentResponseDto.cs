using Application.Dtos.Response.Account;

namespace Application.Dtos.Response.Account.Student;

public class StudentResponseDto : AccountDto
{
    public int? Age { get; set; }
}