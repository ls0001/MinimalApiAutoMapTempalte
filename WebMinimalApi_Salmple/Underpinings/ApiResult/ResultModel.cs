namespace SagUnderpinings;

//统一响应格式
public interface IResultModel
{
    /// <summary>
    ///     是否成功
    /// </summary>
    bool? IsSuccess { get; }

    /// <summary>
    ///     提示信息
    /// </summary>
    string? Message { get; }

    /// <summary>
    ///     业务响应状态码，由具体业务自定义
    /// </summary>
    int? StatusCode { get; set; }

    /// <summary>
    ///     时间戳
    /// </summary>
    long? Timestamp { get; }
}

/// <summary>
///     返回结果模型的泛型接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResultModel<out T> : IResultModel
{
    /// <summary>
    ///     包装的数据
    /// </summary>

    T? Data { get; }
}

/// <summary>
/// 统一响应格式
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResultModel<T> : IResultModel<T>
{
    public ResultModel()
    {
        Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    public ResultModel(bool isSuccess = false, int? code = 999, string? msg = null, T? data = default) : this()
    {
        IsSuccess = isSuccess;
        Message = msg;
        StatusCode = code;
        Data = data;
    }

    #region property

    public bool? IsSuccess { get; set; }

    public string? Message { get; set; }

    public int? StatusCode { get; set; }

    public long? Timestamp { get; }

    public T? Data { get; set; }

    #endregion property

}

