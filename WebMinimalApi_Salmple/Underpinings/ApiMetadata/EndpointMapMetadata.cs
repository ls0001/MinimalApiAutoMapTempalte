using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.OpenApi.Models;

namespace SagUnderpinings;

public class EndpointMapMetadata
{
    private readonly HashSet<object> _metadata = [];
    public RoutePattern? Group { get; private set; }
    public required string Pattern { get; set; }
    public string[]? HttpMethods { get; set; }
    public required Delegate Handler { get; set; }
    public int Order { get; set; } = 0;
    public string? DisplayName { get; private set; }
    public Func<OpenApiOperation, OpenApiOperation>? OpenApiOperator { get; private set; }
    public object[] Metadata { get => _metadata.ToArray(); }

    public EndpointMapMetadata WithMetadata(object? metadata, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(metadata, nameof(metadata));

        if ( metadata != null )
            _metadata.Add(metadata);

        return this;
    }
    public EndpointMapMetadata WithOpenApi(Func<OpenApiOperation, OpenApiOperation>? openApi, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(openApi, nameof(openApi));

        OpenApiOperator = openApi;
        return this;
    }
    public EndpointMapMetadata WithGroup(string? group, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(group, nameof(group));

        if ( string.IsNullOrWhiteSpace(group) == false )
            Group = RoutePatternFactory.Parse(group);
        else
            Group = null;

        return this;
    }
    public EndpointMapMetadata WithName(string? name, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(name, nameof(name));

        if ( string.IsNullOrWhiteSpace(name) == false )
        {
            _metadata.Add(new EndpointNameMetadata(name));
            _metadata.Add(new RouteNameMetadata(name));
        }

        return this;
    }
    public EndpointMapMetadata WithDescription(string? description, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(description, nameof(description));

        if ( string.IsNullOrWhiteSpace(description) == false )
            _metadata.Add(new EndpointDescriptionAttribute(description));

        return this;
    }
    public EndpointMapMetadata WithSummary(string? summary, bool thorwIfNull = false)
    {
        if ( thorwIfNull )
            ThrowNull(summary, nameof(summary));

        if ( string.IsNullOrWhiteSpace(summary) == false )
            _metadata.Add(new EndpointSummaryAttribute(summary));

        return this;
    }
    public EndpointMapMetadata WithDisplayName(string? displayName, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(displayName, nameof(displayName));

        DisplayName = displayName; ;
        return this;
    }
    public EndpointMapMetadata WithTags(string?[]? tags, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(tags, nameof(tags));

        var notNullTags = tags?.Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
        if ( notNullTags != null && notNullTags.Length > 0 )
            _metadata.Add(new TagsAttribute(notNullTags!));

        return this;
    }
    public EndpointMapMetadata WithFormOptions(FormOptions? formOptions, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(formOptions, nameof(formOptions));

        if ( formOptions != null )
            _metadata.Add(formOptions);

        return this;
    }
    public EndpointMapMetadata WithFormMappingOptions(FormMappingOptionsMetadata? formMappingOptions, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(formMappingOptions, nameof(formMappingOptions));

        if ( formMappingOptions != null )
            _metadata.Add(formMappingOptions);

        return this;
    }
    public EndpointMapMetadata WithProduces(ProducesResponseTypeMetadata? produces, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(produces, nameof(produces));

        if ( produces != null )
            _metadata.Add(produces);

        return this;
    }
    public EndpointMapMetadata WithRequestTimeout(RequestTimeoutPolicy? requestTimeout, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(requestTimeout, nameof(requestTimeout));

        if ( requestTimeout != null )
            _metadata.Add(requestTimeout);

        return this;
    }
    public EndpointMapMetadata WithHttpLogging(HttpLoggingOptions? httpLogging, bool throwIfNull = false)
    {
        if ( throwIfNull )
            ThrowNull(httpLogging, nameof(httpLogging));

        if ( httpLogging != null )
            _metadata.Add(httpLogging);

        return this;
    }

    private static void ThrowNull<T>(T? arg, string argName)
    {
        ArgumentNullException.ThrowIfNull(arg, argName);
    }


}