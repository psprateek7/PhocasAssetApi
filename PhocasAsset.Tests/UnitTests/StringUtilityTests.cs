public class StringUtilityTests
{
    [Fact]
    public void DecodeAndSplitId_ValidInput_ReturnsExpectedResults()
    {
        // Arrange
        string input = "MF8yMDE5LTAxLTAxVDIyOjAzOjEwWiMx";
        int expectedAsset = 0;
        string expectedCreatedatTrip = "2019-01-01T22:03:10Z#1";

        // Act
        var result = StringUtility.DecodeAndSplitId(input);

        // Assert
        Assert.Equal(expectedAsset, result.asset);
        Assert.Equal(expectedCreatedatTrip, result.compositeKey);
    }

    [Fact]
    public void DecodeAndSplitId_InvalidBase64_ThrowsFormatException()
    {
        // Arrange
        string input = "UHJhdGVlaw==";

        // Act & Assert
        Assert.Throws<FormatException>(() => StringUtility.DecodeAndSplitId(input));
    }

}
