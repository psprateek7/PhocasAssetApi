public static partial class Logger
{
    [LoggerMessage(LogLevel.Information, "Exception occured while serving {request} with status code {code} and message {message}")]
    public static partial void LogRequestException(this ILogger logger, string request, int code, string message);

    [LoggerMessage(LogLevel.Error, "Databse Exception occured while serving {request} with message {message}")]
    public static partial void LogDynamoException(this ILogger logger, string request, string message);

    [LoggerMessage(LogLevel.Warning, "Unidentifed exception occured while serving {request} from source {source} with message {message}")]
    public static partial void LogUnexpectedException(this ILogger logger, string request, string source, string message);



}