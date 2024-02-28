using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Authentication;

public record EmailCredential
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; init; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; init; } = null!;
}