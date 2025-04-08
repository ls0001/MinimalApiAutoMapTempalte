using System.Collections;

namespace SagUnderpinings;

public class CustomException(int code, string message) : Exception(message)
{
    public int Code { get; private set; } = code;
    public override IDictionary Data { get; } = new Dictionary<string, object?>();
}