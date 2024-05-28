using System.ComponentModel.DataAnnotations;
using System.Globalization;

public class ValidIso8601DateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is string dateString)
        {
            return DateTime.TryParseExact(dateString, "yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out _);
        }
        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The {name} field must be a valid ISO 8601 date-time string (e.g., 2023-05-25T20:22:40Z).";
    }
}