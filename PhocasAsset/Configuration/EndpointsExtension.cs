public static class EndpointsExtension
{
    public static void MapEndpoints(this WebApplication app)
    {

        app.MapGet("/getEventByAssetAndTimeRange", async ([AsParameters] AssetTimeRangeParam queryParam, IAssetService assetService) =>
        {
            return await assetService.GetEventByAssetAndTimeRange(queryParam);
        }).WithDescription(@"Get Events by Asset and TimeRange.
                             Supports upto two days worth of data.
                             To get data for larger duration use getPagedEventByAssetAndTimeRange");

        app.MapGet("/getPagedEventByAssetAndTimeRange", async ([AsParameters] AssetTimeRangeWithLimitParam queryParam, string? nextToken, IAssetService assetService) =>
        {
            return await assetService.GetPagedEventByAssetAndTimeRange(queryParam, nextToken);
        }).WithDescription(@"Get Events by Asset and TimeRange for larger duration(supports pagination).
                             Provide `limit` parameter to set the limit(upto 1000). By default, limit is 25. 
                             This api provides a `nextToken` if number of results are more than the provided limit.
                             Use this token in the subsequent request to get the next records
                             If the token comes back as empty, it means there are no further records to retrive."); ;

        app.MapGet("/getEventByAssetAndTrip", async ([AsParameters] AssetAndTripParam queryParam, IAssetService assetService) =>
        {
            return await assetService.GetEventByAssetAndTrip(queryParam);
        }).WithDescription("Get Events by Asset and Trip.");

        app.MapGet("/getLatestEvents", async (IAssetService assetService) =>
        {
            return await assetService.GetLatestEvents();
        }).WithDescription("Get all the latest events across Assets");

        app.MapGet("/getEventById", async (string id, IAssetService assetService) => await assetService.GetEventById(id));

    }
}