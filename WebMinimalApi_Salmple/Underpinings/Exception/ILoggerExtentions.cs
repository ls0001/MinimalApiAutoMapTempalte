using System.Text.Json;

namespace SagUnderpinings;

internal static class ILoggerExtentions
{
    internal static Task<ILogger> LogException(this ILogger logger, int errCode, Exception exception)
    {
        var data = exception.Data;
        logger.LogError(
           null,              //exception,
           "{{\"Code\":{Code},\r\n" +
           "\"Message\":{Message},\r\n" +
           "\"Source\":{Source},\r\n" +
           "\"StackTrace\":{StackTrace},\r\n" +
           "\"Data\":{Data},\r\n" +
           "\"InnerException\":{InnerException}}}",
           errCode,
           JsonSerializer.Serialize(exception.Message),
           JsonSerializer.Serialize(exception.Source),
           JsonSerializer.Serialize(exception.StackTrace),
           JsonSerializer.Serialize(data),
           GetInnerExceptionJson(exception.InnerException)
       );
        return Task.FromResult(logger);
    }

    public static async Task<ILogger> LogWebException(this ILogger logger, int errCode, Exception exception, HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(exception);
        ArgumentNullException.ThrowIfNull(httpContext);

        var data = exception.Data;
        var request = httpContext.Request;
        var connect = httpContext.Connection;
        var connectInfo = new
        {
            TraceIdentifier = httpContext.TraceIdentifier,
            Remote = connect.RemoteIpAddress?.ToString(),
            RemotePort = connect.RemotePort,
            Local = connect.LocalIpAddress?.ToString(),
            LocalPort = connect.LocalPort
        };

        logger.LogError(
          null,              //exception,
            "{{\"Code\":{Code},\r\n" +
            "\"Message\":{Message},\r\n" +
            "\"Source\":{Source},\r\n" +
            "\"StackTrace\":{StackTrace},\r\n" +
            "\"Path\":\"{Path}\",\r\n" +
            "\"Host\":\"{Host}\",\r\n" +
            "\"Headers\":{Headers},\r\n" +
            "\"RequestBody\":{RequestBody},\r\n" +
            "\"Query\":{Query},\r\n" +
            "\"Connection\":{Connection},\r\n" +
            "\"User\":{User},\r\n" +
            "\"Data\":{Data},\r\n" +
            "\"InnerException\":{InnerException}}}",
            errCode,
            JsonSerializer.Serialize(exception.Message),
            JsonSerializer.Serialize(exception.Source),
            JsonSerializer.Serialize(exception.StackTrace),
            request.Path,
            request.Host,
            JsonSerializer.Serialize(request.Headers),
            request.HasJsonContentType() ? JsonSerializer.Serialize(await request.ReadFromJsonAsync<object>(cancellationToken)) :
            request.HasFormContentType ? JsonSerializer.Serialize(await request.ReadFormAsync(cancellationToken)) : "null",
            JsonSerializer.Serialize(request.Query),
            JsonSerializer.Serialize(connectInfo),
            JsonSerializer.Serialize(httpContext.User.Identity),
            data?.Count > 0 ? JsonSerializer.Serialize(data) : "null",
            GetInnerExceptionJson(exception.InnerException)
        );
        return logger;
    }

    private static string? GetInnerExceptionJson(Exception? exception)
    {
        if ( exception is null )
            return "null";

        var innerException = exception.InnerException == null ? null : new
        {
            exception.InnerException.Message,
            exception.InnerException.Source,
            exception.InnerException.StackTrace,
            exception.InnerException.Data,
            targetSite = exception.InnerException.TargetSite?.ToString()
        };
        return JsonSerializer.Serialize(innerException);
    }
}
