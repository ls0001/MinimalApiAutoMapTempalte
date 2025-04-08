namespace SagUnderpinings;

/// <summary>
/// 用于需要自动注册端点的示例，实现<see cref="IAutoMapEndPoints"/>接口，表示可以被自动注册端点服务扫描
/// </summary>
public class AutoMapEndPointSample : IAutoMapEndPoints
{
    private readonly IConfiguration _config;

    public AutoMapEndPointSample(IConfiguration config) => _config = config;

    /// <summary>
    /// 在这里集中配置所有需要自动注册的API信息,每个端点对应一个EndpointMapInfo对象
    /// </summary>
    /// <returns></returns>
    public EndpointMapInfo[] GetEndpointsMapInfo() => [
        new EndpointMapInfo()
        {
            //必须的路由模式，
            Pattern=nameof(AutoMapEndPointSample),
            //实际的处理方法
            Handler =GetWeatherForecast,     
            //可选：应用于OpenAPI的结果类型架构信息
            ResponseTypeMetadata= new ProducesResponseTypeMetadata(200, typeof(IResultModel<WeatherForecast>), [])
        } 
        //其它可选项
        .WithConfig(
            //配置路径跟据实际的配置文件来填写，也可以直接编码配置剩余的EndPointMapInfo对象的属性值
            _config.GetSection("EndPoints:WeatherForeCast")),

     ];

    /// <summary>
    /// 示例数据
    /// </summary>
    private string[] summaries =
        [
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];
    /// <summary>
    /// 示例处理方法
    /// </summary>
    /// <param name="id">id参数</param>
    /// <param name="context"></param>
    /// <returns></returns>
    public IEnumerable<WeatherForecast> GetWeatherForecast(int? id, HttpContext context)
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Id = id,
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                })
                .ToArray();

        //异常测试
        //CustomException 与系统异常相比，带Code属性（也可以定义更多属性），用于业务逻辑引起的异常，
        var ex = new CustomException(403, "你没有权限！")
        //可选：异常的明细信息。
        {
            Data = {
                ["ke1"]="error",
                ["ke2"]="error2",
            }
        };
        //throw ex;
        //未处理的异常或者系统异常
        //throw new Exception("An error occurred while processing your request");

        return forecast;
    }

}

