using System;
using System.Globalization;

public class DateUtilitiesTests
{
    [Fact]
    public void ParseDateTimes_ValidIsoDates_ReturnsParsedDates()
    {
        // Arrange
        var startIsoDate = "2020-01-01T00:00:00Z";
        var endIsoDate = "2020-01-03T00:00:00Z";

        // Act
        var result = DateUtilities.ParseDateTimes(startIsoDate, endIsoDate);

        // Assert
        Assert.Equal(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), result.startDate);
        Assert.Equal(new DateTime(2020, 1, 3, 0, 0, 0, DateTimeKind.Utc), result.endDate);
    }

    [Theory]
    [InlineData("2020-01D", "2020-01-03T00:00:00Z")]
    [InlineData("2020-01-01T00:00:00Z", "2020-01-03T00:0")]
    [InlineData("2020-01-00:00Z", "Test")]
    public void ParseDateTimes_InvalidIsoDates_ThrowsArgumentException(string startIsoDate, string endIsoDate)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => DateUtilities.ParseDateTimes(startIsoDate, endIsoDate));
    }

    [Theory]
    [InlineData("2020-01-01T00:00:00Z", "2020-01-02T00:00:00Z", true)]  // Start date before end date, should return true
    [InlineData("2020-01-02T00:00:00Z", "2020-01-01T00:00:00Z", false)] // Start date after end date, should return false
    public void IsValidDateRange_ReturnsExpectedResult(string start, string end, bool expected)
    {
        // Arrange
        var startDate = DateTime.Parse(start);
        var endDate = DateTime.Parse(end);

        // Act
        bool result = DateUtilities.IsValidDateRange(startDate, endDate);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("2020-01-01T00:00:00Z", "2020-01-04T00:00:00Z", 2, false)]
    [InlineData("2020-01-01T00:00:00Z", "2020-01-02T00:00:00Z", 2, true)]
    public void IsDateRangeWithMinimumSpan_VariousCases(string start, string end, int minDays, bool expected)
    {
        // Arrange
        var startDate = DateTime.Parse(start, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        var endDate = DateTime.Parse(end, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

        // Act
        bool result = DateUtilities.IsDateRangeWithMinimumSpan(startDate, endDate, minDays);

        // Assert
        Assert.Equal(expected, result);
    }
}
