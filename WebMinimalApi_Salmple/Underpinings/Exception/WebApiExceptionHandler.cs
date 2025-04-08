using System.Collections;
using System.Text.Json;

using Microsoft.AspNetCore.Diagnostics;


namespace SagUnderpinings;

public class WebApiExceptionHandler(ILogger<WebApiException> logger, IWebHostEnvironment environment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int errCode = 999;
        IDictionary? detail = null;
        if ( exception is WebApiException customException )
        {
            errCode = customException.Code;
            detail = customException.Data?.Count > 0 ? customException.Data : null;
        }
        var data = await WebExceptionHandleUtils.GetWebExceptionDataAsync(environment.IsDevelopment(), exception, httpContext, cancellationToken);
        logger.LogWebException(environment, errCode, JsonSerializer.Serialize(data));

        string message = exception.Message;
        if ( !environment.IsDevelopment() )
        {
            data = new Dictionary<string, object?>
            {
                ["TraceIdentifier"] = httpContext.TraceIdentifier,
                ["Detail"] = detail,
            };
        }
        var result = ResultModel.Result(errCode, message, data);
        if ( errCode < 1000 )
            httpContext.Response.StatusCode = errCode;

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
        return true;
    }
}
