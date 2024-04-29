using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Validations;

public class PasswordValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var password = (string)value!;

        var hasDigitAndLetter = new Regex(@"^(?=.*\d)(?=.*[a-zA-Z]).{8,}$");

        return hasDigitAndLetter.IsMatch(password)
            ? ValidationResult.Success
            : new ValidationResult("Password must be at least 8 characters long and contain both letters and numbers");
    }
}