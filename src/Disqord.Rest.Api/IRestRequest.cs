using System;
using Disqord.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

/// <summary>
///     Represents a REST request to the Discord API.
/// </summary>
public interface IRestRequest : IDisposable
{
    /// <summary>
    ///     Gets the route of this request.
    /// </summary>
    IFormattedRoute Route { get; }

    /// <summary>
    ///     Gets the request content associated with this request.
    /// </summary>
    IRestRequestContent? Content { get; }

    /// <summary>
    ///     Gets the request options of this request.
    /// </summary>
    IRestRequestOptions? Options { get; }

    /// <summary>
    ///     Gets or creates the HTTP content from <see cref="Content"/>.
    /// </summary>
    /// <param name="serializer"> The JSON serializer. </param>
    /// <returns>
    ///     The HTTP request content.
    /// </returns>
    HttpRequestContent? GetOrCreateHttpContent(IJsonSerializer serializer);
}
