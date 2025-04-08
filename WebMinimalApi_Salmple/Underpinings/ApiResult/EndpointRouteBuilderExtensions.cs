namespace SagUnderpinings;

public static class EndpointRouteBuilderExtensions
{
    /// <summary>如果端点具有元数据：AutoWrappeResultAttribute，则自动将原始返回结果包装成ResultModel&lt;TResult&gt;</summary>
    public static RouteHandlerBuilder AutoWrapResult(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter(async (context, next) =>
        {
            var result = await next(context); // 执行后继的中间件

            // 若终结点标记了 AutoWrapperResultAttribute，则自动包装
            return (context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<AutoWrapResultAttribute>()) == null
                ? result
                : result switch
                {
                    // 原始结果为非包装类型 => 转换为 WrappedJsonResult
                    not WrappedJsonResult or not IResultModel => new WrappedJsonResult(result),
                    // 已经是包装类型 => 直接返回
                    _ => result
                };
        });
    }
}
