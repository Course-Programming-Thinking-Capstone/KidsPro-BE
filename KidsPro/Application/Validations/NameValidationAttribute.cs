using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Validations;

public class NameValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var name = (string)value!;

        var hasOnlyLettersAndSpaces = new Regex(@"^[a-zA-ZÀ-ỹ ]*$");

        return hasOnlyLettersAndSpaces.IsMatch(name)
            ? ValidationResult.Success
            : new ValidationResult("Name must only contain letters and no numbers or special characters");
    }
}