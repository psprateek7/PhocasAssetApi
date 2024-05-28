using System.Text.Json;

public static class JsonDataReader
{
    public static async Task<(List<AssetTracking>, Dictionary<int, AssetTracking>)> ReadJsonFileAsync(ILogger logger, string filePath)
    {
        var dataPoints = new List<AssetTracking>();
        var latestEvents = new Dictionary<int, AssetTracking>();
        ValidateFilePath(filePath);
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            using FileStream stream = File.OpenRead(filePath);
            using StreamReader reader = new StreamReader(stream);
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                try
                {
                    var dataPoint = JsonSerializer.Deserialize<AssetTracking>(line, options);
                    if (dataPoint != null)
                    {
                        dataPoint.SetSortKey();
                        dataPoint.GenerateIdentifier();
                        dataPoint.SetTripIdCreatedAt();
                        dataPoints.Add(dataPoint);
                        CheckAndUpdateLatestEvent(latestEvents, dataPoint);
                    }
                }
                catch (JsonException jsonEx)
                {
                    logger.LogError($"JSON parse error: {jsonEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Unexpected error while reading data from {filePath}: {ex.Message}");
        }

        return (dataPoints, latestEvents);
    }

    private static void ValidateFilePath(string filePath)
    {
        //application should exit if there is no valid data to operate on.
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified file was not found", filePath);
        }
    }

    private static void CheckAndUpdateLatestEvent(Dictionary<int, AssetTracking> latestEvents, AssetTracking dataPoint)
    {
        if (latestEvents.TryGetValue(dataPoint.Asset, out var existingDataPoint))
        {
            if (DateTime.Parse(dataPoint.CreatedAt) > DateTime.Parse(existingDataPoint.CreatedAt))
            {
                latestEvents[dataPoint.Asset] = dataPoint.CloneAsLatest();
            }
        }
        else
        {
            latestEvents[dataPoint.Asset] = dataPoint.CloneAsLatest();
        }
    }
}