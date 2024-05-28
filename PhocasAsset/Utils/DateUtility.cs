using System.Globalization;

public static class DateUtilities
{
    /// <summary>
    /// Validates that the end date is greater than the start date.
    /// Both dates should be in ISO 8601 format.
    /// </summary>
    /// <param name="startIsoDate">The start date in ISO 8601 format.</param>
    /// <param name="endIsoDate">The end date in ISO 8601 format.</param>
    /// <returns>True if the end date is after the start date; otherwise, false.</returns>
    public static bool IsValidDateRange(string startIsoDate, string endIsoDate)
    {
        if (DateTime.TryParse(startIsoDate, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime startDate) &&
            DateTime.TryParse(endIsoDate, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime endDate))
        {
            return endDate > startDate;
        }
        throw new ArgumentException("One or both of the dates are invalid.");
    }
}
