using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Account.Student;

public class StudentCreateAccountRequest
{
    [Required]
    public int StudendId { get; set; }
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    [EmailAddress]
    public string? ParentEmail { get; set; }
}