
using System.Diagnostics.CodeAnalysis;

namespace SagUnderpinings;

/// <summary>
/// 用于为自动注册端点提供数据的记录类
/// </summary>
public record class EndpointMapInfo
{
    public required string Pattern { get; set; }
    public required Delegate Handler { get; set; }
    public string? Group { get; set; }
    public string[]? HttpMethods { get; set; }
    public bool IsAutoWarpResult { get; set; } = true;
    public string[]? Tags { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public string? DisplayName { get; set; }
    public ProducesResponseTypeMetadata? ResponseTypeMetadata { get; set; }

    public EndpointMapInfo() { }

    [SetsRequiredMembers]
    public EndpointMapInfo(string pattern, Delegate handler)
    {
        Pattern = pattern;
        Handler = handler;
    }
}
