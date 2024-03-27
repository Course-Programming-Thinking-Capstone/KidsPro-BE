using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request.Authentication;

public class StudentLoginRequest
{
    [Required]
    public string Account { get; init; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; init; } = null!;
}