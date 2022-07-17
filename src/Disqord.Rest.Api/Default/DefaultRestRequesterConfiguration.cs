namespace Disqord.Rest.Api.Default;

public class DefaultRestRequesterConfiguration
{
    /// <summary>
    ///     Gets or sets the Discord REST API version the <see cref="DefaultRestRequester"/> will use.
    ///     Defaults to <see cref="Library.Constants.RestApiVersion"/>.
    /// </summary>
    public virtual int Version { get; set; } = Library.Constants.RestApiVersion;
}