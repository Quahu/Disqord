using System;

namespace Disqord.Http;

/// <summary>
///     Represents an HTTP request.
/// </summary>
public interface IHttpRequest : IHeaders, IDisposable
{
    /// <summary>
    ///     Gets the HTTP method of this request.
    /// </summary>
    HttpRequestMethod Method { get; }

    /// <summary>
    ///     Gets the target url of this request.
    /// </summary>
    Uri Uri { get; }

    /// <summary>
    ///     Gets the request content of this request.
    /// </summary>
    HttpRequestContent? Content { get; }

    /// <summary>
    ///     Gets when the response to this request is returned relative to reading its content.
    /// </summary>
    HttpCompletionMode CompletionMode { get; }
}
