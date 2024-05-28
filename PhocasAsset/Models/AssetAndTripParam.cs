using System.ComponentModel.DataAnnotations;

public class AssetAndTripParam
{
    [Range(0, int.MaxValue, ErrorMessage = "Asset must be non-negetive")]
    public int Asset { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Trip must be non-negetive")]
    public int Trip { get; set; }
}