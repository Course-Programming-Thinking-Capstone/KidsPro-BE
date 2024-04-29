using System.ComponentModel.DataAnnotations;

namespace Application.Validations;

public class AdultDayOfBirthValidationAttribute:ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dayOfBirth = (DateTime?)value;
        var yearTime = DateTime.UtcNow.Year-12;

        if (dayOfBirth.HasValue && dayOfBirth.Value.Year > yearTime)
        {
            return new ValidationResult("The adult must be at least 12 years old");
        }

        return ValidationResult.Success;
    }
}