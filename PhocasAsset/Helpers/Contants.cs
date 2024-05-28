public static class Constants
{
    public static class DBConstants
    {
        public const string TableName = "AssetTracking";
        public const string AssetAttribute = "Asset";
        public const string SortKeyAttribute = "SortKey";
        public const string TripIdCreatedAtAttribute = "TripIdCreatedAt";
        public const string StatusAttribute = "Status";
        public const string TripCreatedAtGsi = "TripCreatedAtGsi";
        public const string LatestGSI = "LatestGSI";
        public const string LatestGsiHashKey = "Latest";
    }

    public const int DefaultLimit = 50;
}