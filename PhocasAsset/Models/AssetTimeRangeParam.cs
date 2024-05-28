using System.ComponentModel.DataAnnotations;

public class AssetTimeRangeParam
{
    [Range(0, int.MaxValue, ErrorMessage = "Asset must be non-negetive")]
    public int Asset { get; set; }

    [ValidIso8601Date]
    public string StartDateTime { get; set; }

    [ValidIso8601Date]
    public string EndDateTime { get; set; }
}