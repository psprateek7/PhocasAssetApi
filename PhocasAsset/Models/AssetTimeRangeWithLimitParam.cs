using System.ComponentModel.DataAnnotations;

public class AssetTimeRangeWithLimitParam : AssetTimeRangeParam
{
    [Range(1, 1000, ErrorMessage = "Limit must be between 1 and 1000")]
    public int? Limit { get; set; }

}