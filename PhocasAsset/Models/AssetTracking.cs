public class AssetTracking
{
    public int Asset { get; set; }
    public string SortKey { get; set; }
    public string TripIdCreatedAt { get; set; }
    public string Id { get; set; }
    public int Trip { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Speed { get; set; }
    public string CreatedAt { get; set; }
    public string? Status { get; set; } = null;

    // Method to clone an AssetTracking object with 'Latest' status
    public AssetTracking CloneAsLatest()
    {
        return new AssetTracking
        {
            Asset = this.Asset,
            SortKey = this.SortKey,
            TripIdCreatedAt = this.TripIdCreatedAt,
            Id = this.Id,
            Trip = this.Trip,
            X = this.X,
            Y = this.Y,
            Speed = this.Speed,
            CreatedAt = this.CreatedAt,
            Status = Constants.DBConstants.LatestGsiHashKey
        };
    }

    public AssetDto ToAssetDto()
    {
        return new AssetDto
        {
            Asset = this.Asset,
            Id = this.Id,
            Trip = this.Trip,
            X = this.X,
            Y = this.Y,
            Speed = this.Speed,
            CreatedAt = this.CreatedAt
        };
    }
}