using System.ComponentModel.DataAnnotations;

namespace Application.Validations;

public class MinDateTimeValidationAttribute : ValidationAttribute
{
    private readonly DateTime? _date;

    public MinDateTimeValidationAttribute(DateTime? date)
    {
        _date = date;
    }

    public override bool IsValid(object? value)
    {
        if (this._date != null && value != null)
        {
            var currentValue = (DateTime)value;

            return currentValue >= this._date;
        }

        return true;
    }
}