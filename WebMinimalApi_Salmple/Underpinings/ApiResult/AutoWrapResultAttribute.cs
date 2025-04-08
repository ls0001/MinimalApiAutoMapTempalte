namespace SagUnderpinings;

//标记API端点返回结果需要自动包装成统一格式的特性
[AttributeUsage(AttributeTargets.Method)]
public class AutoWrapResultAttribute : Attribute { }

