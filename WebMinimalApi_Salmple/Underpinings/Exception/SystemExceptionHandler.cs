using System.Collections;

using Microsoft.AspNetCore.Diagnostics;


namespace SagUnderpinings;

public class SystemExceptionHandler(ILogger<CustomException> logger, IWebHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if ( exception is CustomException )
            return false;

        var errCode = 999;
        await logger.LogWebException(errCode, exception, httpContext, cancellationToken);

        IDictionary? data = new Dictionary<string, object?>
        {
            ["traceIdentifier"] = httpContext.TraceIdentifier
        };
        string? message = "An error occurred while attempting to process your request.";
        if ( environment.IsDevelopment() )
        {
            message = exception.Message;
            data = await WebExceptionHandlerUtils.GetWebExceptionDataAsync(exception, httpContext, cancellationToken);
            data["code"] = errCode;
        }

        var result = ResultModel.Failed(message, errCode, data);
        if ( errCode < 1000 )
            httpContext.Response.StatusCode = errCode;

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;

    }
}
