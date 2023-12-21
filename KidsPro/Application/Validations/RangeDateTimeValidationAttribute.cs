using System.ComponentModel.DataAnnotations;

namespace Application.Validations;

public class RangeDateTimeValidationAttribute : ValidationAttribute
{
    private readonly DateTime? _minDate;
    private readonly DateTime? _maxDate;

    public RangeDateTimeValidationAttribute(DateTime? minDate, DateTime? maxDate)
    {
        _minDate = minDate;
        _maxDate = maxDate;
    }

    public override bool IsValid(object? value)
    {
        if (value != null)
        {
            if (_minDate == null || _maxDate == null)
                throw new ArgumentException();
            var currentValue = (DateTime)value;
            return _minDate <= currentValue && currentValue >= _maxDate;
        }

        return true;
    }
}