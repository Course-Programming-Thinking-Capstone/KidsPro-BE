using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Authentication;

public record EmailRegisterDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; init; } = null!;

    [Required] public string FullName { get; set; } = null!;

    [Required] public string Password { get; set; } = null!;
}