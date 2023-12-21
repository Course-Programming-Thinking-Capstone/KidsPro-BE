using System.ComponentModel.DataAnnotations;

namespace Application.Validations;

public class StartSaleDateValidationAttribute:ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var startSaleDate = (DateTime?)value;
        var endSaleDate = (DateTime?)validationContext.ObjectType.GetProperty("EndSaleDate")?.GetValue(validationContext.ObjectInstance);

        if (startSaleDate.HasValue && endSaleDate.HasValue && startSaleDate > endSaleDate)
        {
            return new ValidationResult("Start date must be less than or equal to end date.");
        }

        return ValidationResult.Success;
    }
}