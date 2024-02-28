using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Authentication;

public record PhoneNumberRegisterDto
{
    [Required]
    [RegularExpression(@"^(0[135789])([0-9]{8,9})\b$",
        ErrorMessage = "Phone number is in incorrect format.")]
    public string PhoneNumber { get; init; } = null!;

    [Required] public string FullName { get; set; } = null!;

    [Required] public string Password { get; set; } = null!;
};