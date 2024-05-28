public interface IDataAccess
{
    public Task<TableState> CreateTableIfNotExistAsync();
    public Task SeedDataAsync();
    public Task<AssetTracking> GetEventById(int asset, string sortKey);
    public Task<List<AssetTracking>> GetEventsByAssetAndTimeRange(int asset, string startDateTime, string endDateTime);
    public Task<(List<AssetTracking>, string)> GetPagedEventsByAssetAndTimeRange(int asset, string startDateTime, string endDateTime, int limit, string nextToken);
    public Task<List<AssetTracking>> GetEventByAssetAndTrip(int assetId, int tripId);
    public Task<List<AssetTracking>> GetLatestEvents();
}