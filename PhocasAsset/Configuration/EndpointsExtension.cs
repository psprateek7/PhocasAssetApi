public static class EndpointsExtension
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/getEventById", async (string id, IAssetService assetService) => await assetService.GetEventById(id));

        app.MapGet("/getEventByAssetAndTimeRange", async ([AsParameters] AssetTimeRangeParam queryParam, IAssetService assetService) =>
        {
            return await assetService.GetEventByAssetAndTimeRange(queryParam);
        });

        app.MapGet("/getPagedEventByAssetAndTimeRange", async ([AsParameters] AssetTimeRangeWithLimitParam queryParam, string? nextToken, IAssetService assetService) =>
        {
            return await assetService.GetPagedEventByAssetAndTimeRange(queryParam, nextToken);
        });

        app.MapGet("/getEventByAssetAndTrip", async ([AsParameters] AssetAndTripParam queryParam, IAssetService assetService) =>
        {
            return await assetService.GetEventByAssetAndTrip(queryParam);
        });

        app.MapGet("/getLatestEvents", async (IAssetService assetService) =>
        {
            return await assetService.GetLatestEvents();
        });

    }
}