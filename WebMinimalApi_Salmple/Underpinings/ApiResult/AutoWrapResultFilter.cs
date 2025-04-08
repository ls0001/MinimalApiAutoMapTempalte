namespace SagUnderpinings;

public class AutoWrapResultFilter : IEndpointFilter
{

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context); // 执行后继的中间件
        // 若终结点标记了 AutoWrappeResultAttribute，则自动包装
        return (context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<AutoWrapResultAttribute>()) == null
            ? result
            : result switch
            {
                // 原始结果为非包装类型 => 转换为 WrappedJsonResult
                not WrappedJsonResult => new WrappedJsonResult(result),
                // 已经是包装类型 => 直接返回
                _ => result
            };
    }
}

