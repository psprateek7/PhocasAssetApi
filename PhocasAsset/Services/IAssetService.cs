public interface IAssetService
{
    Task<AssetDto> GetEventById(string eventId);

    Task<List<AssetDto>> GetEventByAssetAndTimeRange(AssetTimeRangeParam queryParam);

    public Task<PaginatedResponse<AssetDto>> GetPagedEventByAssetAndTimeRange(AssetTimeRangeWithLimitParam queryParam, string nextToken);

    public Task<List<AssetDto>> GetEventByAssetAndTrip(AssetAndTripParam param);

    public Task<List<AssetDto>> GetLatestEvents();
}