using System.ComponentModel.DataAnnotations;
using Application.Validations;

namespace Application.Dtos.Request.Account;

public record ChangePasswordDto
{
    [PasswordValidation] [Required] public string OldPassword { get; set; } = null!;
    [PasswordValidation] [Required] public string NewPassword { get; set; } = null!;
}