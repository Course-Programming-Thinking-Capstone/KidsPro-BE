using System.ComponentModel.DataAnnotations;
using Application.Validations;

namespace Application.Dtos.Request.Authentication;

public record EmailRegisterDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; init; } = null!;

    [NameValidation] [Required] public string FullName { get; set; } = null!;

    [PasswordValidation] [Required] public string Password { get; set; } = null!;
    [PasswordValidation] [Required] public string RePassword { get; set; } = null!;
}