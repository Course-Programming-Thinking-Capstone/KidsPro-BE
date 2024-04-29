using System.ComponentModel.DataAnnotations;

namespace Application.Validations;

public class StudentDayOfBirthValidationAttribute:ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dayOfBirth = (DateTime?)value;
        var yearTime = DateTime.UtcNow.Year-5;

        if (dayOfBirth.HasValue && dayOfBirth.Value.Year > yearTime)
        {
            return new ValidationResult("The student must be at least 5 years old");
        }

        return ValidationResult.Success;
    }
}