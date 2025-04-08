using System.Text.Json;


namespace SagUnderpinings;

public class WebExceptionHandlerUtils
{
    public static async Task<Dictionary<string, object?>> GetWebExceptionDataAsync(Exception exception, HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        var errCode = exception.HResult;

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
        var user = new
        {
            Name = httpContext.User.Identity?.Name,
            IsAuthenticated = httpContext.User.Identity?.IsAuthenticated
        };
        var body = request.HasJsonContentType() ? JsonSerializer.Serialize(await request.ReadFromJsonAsync<object>(cancellationToken))
              : request.HasFormContentType ? JsonSerializer.Serialize(await request.ReadFormAsync(cancellationToken)) : "null";

        var data = new Dictionary<string, object?>
        {
            ["code"] = errCode,
            ["source"] = exception.Source,
            ["details"] = exception.Data?.Count > 0 ? exception.Data : null,
            ["targetSite"] = exception.TargetSite?.ToString(),
            ["stackTrace"] = exception.StackTrace,
            ["headers"] = request.Headers,
            ["host"] = request.Host.Value,
            ["path"] = request.Path.Value,
            ["query"] = request.Query,
            ["user"] = user,
            ["connection"] = connectInfo,
            ["body"] = body,
            ["innerException"] = exception.InnerException
        };
        return data;
    }

}