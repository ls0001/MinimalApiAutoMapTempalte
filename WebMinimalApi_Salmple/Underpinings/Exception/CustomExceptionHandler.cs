using Microsoft.AspNetCore.Diagnostics;


namespace SagUnderpinings;

public class CustomExceptionHandler(ILogger<CustomException> logger, IWebHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if ( exception is not CustomException customException )
            return false;

        var errCode = customException.Code;
        var data = new Dictionary<string, object?>(customException.Data as Dictionary<string, object?> ?? []);
        data["traceIdentifier"] = httpContext.TraceIdentifier;
        await logger.LogWebException(errCode, exception, httpContext, cancellationToken);

        if ( environment.IsDevelopment() )
        {
            data = await WebExceptionHandlerUtils.GetWebExceptionDataAsync(exception, httpContext, cancellationToken);
            data["code"] = errCode;
        }

        var result = ResultModel.Result(errCode, customException.Message, data);
        if ( errCode < 1000 )
            httpContext.Response.StatusCode = errCode;

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }
}
