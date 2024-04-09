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
    public int OrderId { get; set; }
    [Required]
    public int ClassId { get; set; }
    
}