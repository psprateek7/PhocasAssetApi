public static class StringUtility
{
    public static (int asset, string compositeKey) DecodeAndSplitId(string encodedId)
    {
        // Decode the Base64 encoded string
        string decodedId = DataEncoder.Decode(encodedId);

        // Split the decoded ID to extract the asset and createdAt#Trip
        string[] parts = decodedId.Split('_');
        if (parts.Length != 2 || !parts[1].Contains("#"))
        {
            throw new FormatException("Invalid ID format.");
        }

        int asset = int.Parse(parts[0]);  // Convert asset part to integer
        string compositeKey = parts[1];  // This should be in the format createdAt#Trip

        return (asset, compositeKey);
    }
}