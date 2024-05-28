public interface IAssetService
{
    /// <summary>
    /// Retrieves a single event by its ID.
    /// </summary>
    /// <param name="eventId">The unique identifier for the event.</param>
    /// <returns><see cref="AssetDto"/></returns>
    Task<AssetDto> GetEventById(string eventId);

    /// <summary>
    /// Retrieves a list of events for a specific asset within a given time range.
    /// </summary>
    /// <param name="queryParam">The parameters defining the asset and the time range for the query.</param>
    /// <returns>List of <see cref="AssetDto"/></returns>
    Task<List<AssetDto>> GetEventByAssetAndTimeRange(AssetTimeRangeParam queryParam);

    /// <summary>
    /// Retrieves a paginated list of events for a specific asset within a given time range.
    /// </summary>
    /// <param name="queryParam">The parameters including asset ID, time range, and page limit.</param>
    /// <param name="nextToken">The pagination token for fetching the next page of results.</param>
    /// <returns>List of <see cref="AssetDto"/> and a continuation token.</returns>
    public Task<PaginatedResponse<AssetDto>> GetPagedEventByAssetAndTimeRange(AssetTimeRangeWithLimitParam queryParam, string nextToken);

    /// <summary>
    /// Retrieves a list of events for a specific asset and a specific trip.
    /// </summary>
    /// <param name="param">The parameters defining the asset and the trip.</param>
    /// <returns>List of <see cref="AssetDto"/></returns>
    public Task<List<AssetDto>> GetEventByAssetAndTrip(AssetAndTripParam param);

    /// <summary>
    /// Retrieves a list of the most recent events for all assets.
    /// </summary>
    /// <returns>List of <see cref="AssetDto"/></returns>
    public Task<List<AssetDto>> GetLatestEvents();
}