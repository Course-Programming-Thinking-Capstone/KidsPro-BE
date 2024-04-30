using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Validations;

public class PhoneValidationAttribute: ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var name = (string)value!;

        var isValidPhoneNumber = new Regex(@"^\+?\d{10,15}$");

        return isValidPhoneNumber.IsMatch(name)
            ? ValidationResult.Success
            : new ValidationResult("Phone number must be between 10 and 15 digits long and may start with a plus sign");
    }
}