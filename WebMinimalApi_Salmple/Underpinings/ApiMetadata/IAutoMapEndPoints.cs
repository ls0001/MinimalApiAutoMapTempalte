namespace SagUnderpinings;

/// <summary>
/// ��ʾ�ṩ�˽������Զ�ע��Ϊ�˵��<see cref="EndpointMapInfo"/>��Ϣ
/// </summary>
public interface IAutoMapEndPoints
{
    /// <summary>
    /// ��ʵ������ע����Ҫ��ΪAPI�˵�ĺ�����Ϊÿ���˵��ṩһ��<see cref="EndpointMapInfo"/>����
    /// </summary>
    /// <returns></returns>
    public EndpointMapInfo[] GetEndpointsMapInfo();
}
