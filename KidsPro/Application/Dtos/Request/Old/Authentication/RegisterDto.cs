using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Old.Authentication;

public record RegisterDto
{
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^(0[135789])([0-9]{8,9})\b$",
        ErrorMessage = "Phone number is in incorrect format.")]
    [StringLength(11)]
    public string PhoneNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(30)]
    public string FullName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(4, ErrorMessage = "Password must have at least 4 character")]
    public string Password { get; init; } = string.Empty;
}