using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Account;

public record ChangePasswordDto
{
    [Required] public string OldPassword { get; set; } = null!;

    [Required] public string NewPassword { get; set; } = null!;
}