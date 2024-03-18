using System.Globalization;

namespace Application.Utils;

public static class DateUtils
{
    public static string FormatDateTimeToDateV1(DateTime? date) => date?.ToString("dd/MM/yyyy") ?? "";
    public static string FormatDateTimeToDateV2(DateTime? date) => date?.ToString("dd/MM/yyyy dddd") ?? "";
    public static string FormatDateTimeToDateV3(DateTime? date) => date?.ToString("yyyy/MM/dd dddd") ?? "";
    public static string FormatDateTimeToDatetimeV1(DateTime? date) => date?.ToString("dd/MM/yyyy HH:mm:ss") ?? "";
    public static string FormatDateTimeToDatetimeV2(DateTime? date) => date?.ToString("dd/MM/yyyy HH:mm:ss dddd") ?? "";
    public static string FormatDateTimeToDatetimeV3(DateTime? date) => date?.ToString("yyyy/MM/dd HH:mm:ss dddd") ?? "";

    public static DateTime? ConvertStringToDateTimeV1(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString))
        {
            return null;
        }

        if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var date))
        {
            return date;
        }

        return null;
    }

    public static DateTime? ConvertStringToDateTimeV2(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString))
        {
            return null;
        }

        if (DateTime.TryParseExact(dateString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var date))
        {
            return date;
        }

        return null;
    }

    public static DateTime? ConvertStringToDateTimeV3(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString))
        {
            return null;
        }

        if (DateTime.TryParseExact(dateString, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var date))
        {
            return date;
        }

        return null;
    }
}