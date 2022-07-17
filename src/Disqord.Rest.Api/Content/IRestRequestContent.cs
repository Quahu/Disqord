using Disqord.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

/// <summary>
///     Represents the request content of a REST request.
/// </summary>
public interface IRestRequestContent
{
    /// <summary>
    ///     Creates the HTTP request content from this request content.
    /// </summary>
    /// <param name="serializer"> The JSON serializer. </param>
    /// <param name="options"> The optional request options. </param>
    /// <returns>
    ///     A new <see cref="HttpRequestContent"/>.
    /// </returns>
    HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions? options = null);

    /// <summary>
    ///     Ensures that this request content is valid to be sent to the Discord API.
    /// </summary>
    void Validate();
}
