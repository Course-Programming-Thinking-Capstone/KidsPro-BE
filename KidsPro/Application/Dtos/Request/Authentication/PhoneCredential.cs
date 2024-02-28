using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Authentication;

public record PhoneCredential
{
    [Required]
    [RegularExpression(@"^(0[135789])([0-9]{8,9})\b$",
        ErrorMessage = "Phone number is in incorrect format.")]
    public string PhoneNumber { get; init; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; init; } = null!;
}