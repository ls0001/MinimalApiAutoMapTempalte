namespace SagUnderpinings;

/// <summary>
/// 表示提供了将函数自动注册为端点的<see cref="EndpointMapInfo"/>信息
/// </summary>
public interface IAutoMapEndPoints
{
    /// <summary>
    /// 在实现类中注册需要作为API端点的函数，为每个端点提供一个<see cref="EndpointMapInfo"/>对象
    /// </summary>
    /// <returns></returns>
    public EndpointMapInfo[] GetEndpointsMapInfo();
}
