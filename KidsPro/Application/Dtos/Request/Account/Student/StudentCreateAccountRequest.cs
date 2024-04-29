using System.ComponentModel.DataAnnotations;
using Application.Validations;

namespace Application.Dtos.Request.Account.Student;

public class StudentCreateAccountRequest
{
    [Required] public int StudendId { get; set; }
    [NameValidation] [Required] public string? UserName { get; set; }
    [PasswordValidation] [Required] public string? Password { get; set; }
    [Required] public int OrderId { get; set; }
    [Required] public int ClassId { get; set; }
}