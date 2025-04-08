
namespace SagUnderpinings;

/// <summary>
///     返回结果的工厂
/// </summary>
public static class ResultModel
{
    private const string _ok = "OK";
    private const int _okCode = 200;
    private const string _failed = "Failed";
    private const int _failedCode = 500;
    private const string _notFound = "Not Found";

    /// <summary>
    ///     数据已存在
    /// </summary>
    /// <returns></returns>
    public static IResultModel<string> HasExists => Failed<string>("The data already exists");

    /// <summary>
    ///     数据不存在
    /// </summary>
    public static IResultModel<string> NotExists => Failed<string>("The data doesn't exist");

    /// <summary>
    ///     根据布尔值返回结果
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static async Task<IResultModel<string>> Result(Task<bool> success)
    {
        return Result(await success);
    }

    /// <summary>
    ///     根据布尔值返回结果
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static IResultModel<string> Result(bool success)
    {
        return success ? Success<string>() : Failed<string>();
    }

    /// <summary>
    ///     成功:带数据
    /// </summary>
    /// <param name="data">返回数据</param>
    /// <returns></returns>
    public static IResultModel<T> Success<T>(string? msg = null, T? data = default)
    {
        return new ResultModel<T>(true, _okCode, msg, data);
    }

    /// <summary>
    ///     失败:带自定义类型(T)明细信息
    /// </summary>
    /// <param name="error">错误信息</param>
    /// <returns></returns>
    public static IResultModel<T> Failed<T>(string? error = null, int? code = _failedCode, T? data = default)
    {
        return new ResultModel<T>(false, code, error, data);
    }

    /// <summary>
    ///     成功:带数据,必须确保dataFactory成功或者已经处理了异常
    /// </summary>
    /// <param name="msg">成功提示文本</param>
    /// <param name="dataFactory">返回数据的工厂</param>
    /// <returns></returns>
    public static async Task<IResultModel<T>> SuccessAsync<T>(string? msg, Task<T> dataFactory)
    {
        return new ResultModel<T>(true, _okCode, msg, await dataFactory);
    }

    /// <summary>
    ///     失败:带明细的问题信息
    /// </summary>
    /// <param name="error">错误信息</param>
    /// <returns></returns>
    public static IResultModel<Dictionary<string, object?>> Failed(string? error = null, int? code = _failedCode, Dictionary<string, object?>? data = default)
    {
        return new ResultModel<Dictionary<string, object?>>(false, code, error, data);
    }

    public static IResultModel<T> Result<T>(int code, string? msg, T? data)
    {
        return new ResultModel<T>(code == _okCode, code, msg, data);
    }

}

