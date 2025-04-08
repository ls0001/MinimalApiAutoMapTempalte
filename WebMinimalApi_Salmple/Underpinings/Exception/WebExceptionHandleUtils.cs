using System.Diagnostics.CodeAnalysis;
using System.Text.Json;


namespace SagUnderpinings;

public class WebExceptionHandleUtils
{
    public static async Task<Dictionary<string, object?>> GetWebExceptionDataAsync(bool includeBody, Exception exception, HttpContext httpContext, CancellationToken cancellationToken = default)
    {
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
        var data = GetExceptionLogData(exception);
        data["Host"] = request.Host.Value;
        data["Headers"] = request.Headers;
        data["Path"] = request.Path.Value;
        data["Query"] = request.Query;
        data["User"] = user;
        data["Connection"] = connectInfo;

        if ( includeBody && request.Body.CanRead )
        {
            string? body = null;
            try
            {
                if ( request.HasJsonContentType() || HasApplicationFormContentType(httpContext) )
                {
                    request.Body.Position = 0;
                    using var reader = new StreamReader(request.Body, null, true, 4096, true);
                    body = await reader.ReadToEndAsync(cancellationToken);
                }
                else if ( httpContext.Request.Form != null )
                {
                    body = JsonSerializer.Serialize(httpContext.Request.Form);
                }
            }
            catch
            {
                body = "读取body内容失败";
            }
            data["Body"] = string.IsNullOrWhiteSpace(body) ? null : body;
        }

        return data;

    }

    private static Dictionary<string, object?> GetExceptionLogData(Exception exception)
    {
        return new Dictionary<string, object?>
        {
            ["Message"] = exception.Message,
            ["Source"] = exception.Source,
            ["TargetSite"] = exception.TargetSite?.ToString(),
            ["StackTrace"] = exception.StackTrace,
            ["Detail"] = exception.Data?.Count > 0 ? exception.Data : null,
            ["InnerException"] = exception.InnerException is null ? null : GetExceptionLogData(exception.InnerException)
        };
    }

    private static bool HasApplicationFormContentType([NotNullWhen(true)] HttpContext httpContext)
    {
        return httpContext.Request.ContentType?.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) ?? false;
    }

}