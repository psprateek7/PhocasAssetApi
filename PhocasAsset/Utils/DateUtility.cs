using System.Globalization;

public static class DateUtilities
{
    /// <summary>
    /// Tries to parse two ISO 8601 formatted date strings into DateTime objects.
    /// </summary>
    /// <param name="startIsoDate">The start date in ISO 8601 format.</param>
    /// <param name="endIsoDate">The end date in ISO 8601 format.</param>
    /// <returns>A tuple containing the parsed start and end DateTime objects.</returns>
    /// <exception cref="ArgumentException">Thrown if one or both date strings cannot be parsed.</exception>
    public static (DateTime startDate, DateTime endDate) ParseDateTimes(string startIsoDate, string endIsoDate)
    {
        if (DateTime.TryParse(startIsoDate, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime startDate) &&
            DateTime.TryParse(endIsoDate, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime endDate))
        {
            return (startDate, endDate);
        }
        throw new ArgumentException("One or both of the dates are invalid.");
    }

    /// <summary>
    /// Validates that the end date is greater than the start date.
    /// </summary>
    /// <param name="startDate">The start date as a DateTime object.</param>
    /// <param name="endDate">The end date as a DateTime object.</param>
    /// <returns>True if the end date is after the start date; otherwise, false.</returns>
    public static bool IsValidDateRange(DateTime startDate, DateTime endDate) => endDate > startDate;


    /// <summary>
    /// Determines if the difference between two dates is less than or equal to a specified number of days.
    /// </summary>
    /// <param name="startDate">The start date as a DateTime object.</param>
    /// <param name="endDate">The end date as a DateTime object.</param>
    /// <param name="minDaysDifference">The maximum number of days allowed between the start and end dates. The default value is 2 days.</param>
    /// <returns>True if the difference between the start and end dates is less than or equal to the specified number of days; otherwise, false.</returns>
    public static bool IsDateRangeWithMinimumSpan(DateTime startDate, DateTime endDate, int minDaysDifference = 2)
    {
        // Check if the difference between the dates is more than the specified minimum days
        TimeSpan dateDifference = endDate - startDate;
        if (dateDifference.TotalDays <= minDaysDifference)
        {
            return true;
        }
        return false;
    }
}
