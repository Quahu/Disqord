using System;
using System.IO;
using System.Threading.Tasks;

namespace Disqord.Http;

/// <summary>
///     Represents an HTTP response.
/// </summary>
public interface IHttpResponse : IHeaders, IDisposable
{
    /// <summary>
    ///     Gets the HTTP status code of this response.
    /// </summary>
    HttpResponseStatusCode StatusCode { get; }

    /// <summary>
    ///     Gets the HTTP reason phrase sent along status codes.
    /// </summary>
    string? ReasonPhrase { get; }

    /// <summary>
    ///     Reads the HTTP body content of this response.
    /// </summary>
    /// <returns> The stream representing the body. </returns>
    Task<Stream> ReadAsync();
}
