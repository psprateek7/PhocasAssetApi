using System.Net;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

public class AssetService : IAssetService
{
    private readonly IDataAccess _dataAccess;

    private readonly ILogger<AssetService> _logger;
    public AssetService(IDataAccess dataAccess, ILogger<AssetService> logger)
    {
        _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess)); ;
        _logger = logger;
    }

    public async Task<AssetDto> GetEventById(string eventId)
    {
        (int asset, string sortKey) = StringUtility.DecodeAndSplitId(eventId);
        var assetEvent = await _dataAccess.GetEventById(asset, sortKey);
        if (assetEvent is null)
        {
            throw new HttpException(HttpStatusCode.NotFound, $"No Events found for event ID : {eventId}");
        }

        return assetEvent.ToAssetDto(); ;
    }

    public async Task<List<AssetDto>> GetEventByAssetAndTimeRange(AssetTimeRangeParam param)
    {
        RequestValidator.Validate(param);
        var events = await _dataAccess.GetEventsByAssetAndTimeRange(param.Asset, param.StartDateTime, param.EndDateTime);
        if (events is null || events.Count == 0)
        {
            throw new HttpException(HttpStatusCode.NotFound, $"No Events found for Asset : {param.Asset} and DateRange {param.StartDateTime} - {param.EndDateTime}");
        }
        return events.Select(at => at.ToAssetDto()).ToList(); ;
    }

    public async Task<PaginatedResponse<AssetDto>> GetPagedEventByAssetAndTimeRange(AssetTimeRangeWithLimitParam param, string nextToken)
    {
        var effectiveLimit = param.Limit ?? 25;
        RequestValidator.Validate(param);
        if (!DateUtilities.IsValidDateRange(param.StartDateTime, param.EndDateTime))
        {
            throw new HttpException(HttpStatusCode.BadRequest, $"Invalid Date range. End Date can not be before the Start Date");
        }
        if (!string.IsNullOrEmpty(nextToken))
        {
            nextToken = DataEncoder.Decode(nextToken);
        }

        var (events, token) = await _dataAccess.GetPagedEventsByAssetAndTimeRange(param.Asset, param.StartDateTime, param.EndDateTime, effectiveLimit, nextToken);
        var encodedToken = DataEncoder.Encode(token);

        var eventsDto = events.Select(at => at.ToAssetDto()).ToList();
        return new PaginatedResponse<AssetDto>(eventsDto, encodedToken);
    }

    public async Task<List<AssetDto>> GetEventByAssetAndTrip(AssetAndTripParam param)
    {
        RequestValidator.Validate(param);
        var events = await _dataAccess.GetEventByAssetAndTrip(param.Asset, param.Trip);
        if (events is null || events.Count == 0)
        {
            throw new HttpException(HttpStatusCode.NotFound, $"No Events found for Asset : {param.Asset} and Trip: {param.Trip}");
        }
        return events.Select(at => at.ToAssetDto()).ToList();
    }

    public async Task<List<AssetDto>> GetLatestEvents()
    {
        var events = await _dataAccess.GetLatestEvents();
        if (events is null || events.Count == 0)
        {
            throw new HttpException(HttpStatusCode.NotFound, $"No latest events found");
        }
        return events.Select(at => at.ToAssetDto()).ToList();
    }
}