using Application.Dtos.Response.Account;

namespace Application.Dtos.Response.Account.Student;

public class StudentResponse : AccountDto
{
    public int? Age { get; set; }
    public int ParentId { get; set; }
    public string? ParentName { get; set; }
}