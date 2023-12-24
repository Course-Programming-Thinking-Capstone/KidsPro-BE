using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Dtos.Request.Authentication;

public record RegisterDto
{
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^(0[35789])([0-9]{8,9})\b$",
        ErrorMessage = "Phone number is in incorrect format.")]
    [StringLength(11)]
    public string PhoneNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100)]
    public string FullName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(4, ErrorMessage = "Password must have at least 4 character")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [MinLength(4, ErrorMessage = "Password must have at least 4 character")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public Gender? Gender { get; set; }
}