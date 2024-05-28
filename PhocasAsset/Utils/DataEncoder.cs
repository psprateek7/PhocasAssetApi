public static class DataEncoder
{
    public static string Encode(string data)
    {
        if (string.IsNullOrEmpty(data))
            return string.Empty;

        var dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
        return Convert.ToBase64String(dataBytes);
    }

    public static string Decode(string encodedData)
    {
        if (string.IsNullOrEmpty(encodedData))
            return string.Empty;

        var base64EncodedBytes = Convert.FromBase64String(encodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}