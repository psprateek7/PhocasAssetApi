public static class AssetExtensions
{
    public static void SetSortKey(this AssetTracking assetTracking)
    {
        assetTracking.SortKey = $"{assetTracking.CreatedAt}#{assetTracking.Trip}";
    }

    public static void SetTripIdCreatedAt(this AssetTracking assetTracking)
    {
        assetTracking.TripIdCreatedAt = $"{assetTracking.Trip}#{assetTracking.CreatedAt}";
    }

    public static string GenerateIdentifier(this AssetTracking assetTracking)
    {
        return DataEncoder.Encode($"{assetTracking.Asset}_{assetTracking.CreatedAt}#{assetTracking.Trip}");
    }
}
