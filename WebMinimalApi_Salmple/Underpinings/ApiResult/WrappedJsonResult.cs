namespace SagUnderpinings;

/// <summary>
/// API返回结果的包装器
/// </summary>
/// <param name="Data"></param>
/// <param name="Code"></param>
/// <param name="Message"></param>
public record WrappedJsonResult(object? Data, int? Code = default, string? Message = null) : IResult
{
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        var response = httpContext.Response;
        var result = ResultModel.Result(Code ?? response.StatusCode, Message, Data);
        await response.WriteAsJsonAsync(result);
    }
}
