using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        switch (exception)
        {
            case HttpException httpException:
                _logger.LogRequestException(context.Request?.Path ?? "", httpException.StatusCode, httpException.Message);
                context.Response.StatusCode = httpException.StatusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = httpException.Message }));
                break;

            case AmazonDynamoDBException dynamoDbException:
                _logger.LogDynamoException(context.Request?.Path ?? "", dynamoDbException.Message);
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(JsonSerializer.Serialize(new { message = "An unexpected error occurred." }));
                break;

            case FormatException formatException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = formatException.Message }));
                break;

            default:
                _logger.LogUnexpectedException(context.Request?.Path ?? "", exception.Source ?? "", exception.Message);
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await response.WriteAsync(JsonSerializer.Serialize(new { message = "An unexpected error occurred. Please try again later" }));
                break;
        }
    }
}
