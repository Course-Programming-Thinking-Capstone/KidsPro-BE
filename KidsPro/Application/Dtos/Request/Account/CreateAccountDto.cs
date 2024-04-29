using System.ComponentModel.DataAnnotations;
using Application.Validations;
using Domain.Enums;

namespace Application.Dtos.Request.Account;

public record CreateAccountDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; init; } = null!;

    [NameValidation] [Required] public string FullName { get; init; } = null!;

    [AdultDayOfBirthValidation]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; init; }

    public Gender? Gender { get; init; }

    [PhoneValidation] public string? PhoneNumber { get; init; }
    [Required] public string Role { get; init; } = null!;
};