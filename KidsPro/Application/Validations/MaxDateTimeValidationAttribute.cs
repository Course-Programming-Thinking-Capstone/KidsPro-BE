using System.ComponentModel.DataAnnotations;

namespace Application.Validations;

public class MaxDateTimeValidationAttribute : ValidationAttribute
{
    private readonly DateTime? _date;

    public MaxDateTimeValidationAttribute(DateTime? date)
    {
        _date = date;
    }

    public override bool IsValid(object? value)
    {
        if (this._date != null && value != null)
        {
            var currentValue = (DateTime)value;

            return currentValue <= this._date;
        }

        return true;
    }
}