using Domain.Enums;

namespace Application.Utils;

public static class EnumUtils
{
    public static UserStatus ConvertToUserStatus(string value)
    {
        if (Enum.TryParse(value, true, out UserStatus status))
        {
            return status;
        }
        else
        {
            throw new ArgumentException($"Invalid UserStatus value: {value}");
        }
    }
}