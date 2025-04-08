namespace SagUnderpinings;

public static class EndpointMapInfoExtensions
{
    public static EndpointMapInfo WithConfig(this EndpointMapInfo endpointMapInfo, IConfiguration config)
    {
        var pattern = config.GetValue<string>("Pattern", endpointMapInfo.Pattern);
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern, nameof(endpointMapInfo.Pattern));
        endpointMapInfo.Pattern = pattern;
        endpointMapInfo.Group = config.GetValue<string?>("Group", null);
        endpointMapInfo.HttpMethods = config.GetSection("HttpMethods").Get<string[]>() ?? [HttpMethods.Post];
        endpointMapInfo.Name = config.GetValue<string?>("Name", null);
        endpointMapInfo.Summary = config.GetValue<string?>("Summary", null);
        endpointMapInfo.DisplayName = config.GetValue<string?>("DisplayName", null);
        endpointMapInfo.Description = config.GetValue<string?>("Description", null);
        endpointMapInfo.Tags = config.GetSection("Tags").Get<string[]?>();
        endpointMapInfo.IsAutoWarpResult = config.GetValue<bool>("IsAutoWarpResult", true);
        return endpointMapInfo;
    }

}