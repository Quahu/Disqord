using System;
using Qommon;

namespace Disqord.Http.Default;

public class DefaultHttpRequest : HeadersBase, IHttpRequest
{
    /// <inheritdoc/>
    public HttpRequestMethod Method { get; }

    /// <inheritdoc/>
    public Uri Uri { get; }

    /// <inheritdoc/>
    public HttpRequestContent? Content { get; }

    /// <inheritdoc/>
    public HttpCompletionMode CompletionMode { get; }

    /// <summary>
    ///     Instantiates a new <see cref="DefaultHttpRequest"/>.
    /// </summary>
    /// <param name="method"> The HTTP method to use. </param>
    /// <param name="uri"> The target url. </param>
    /// <param name="content"> The content. </param>
    /// <param name="completionMode"> When the response is returned relative to reading its content. </param>
    public DefaultHttpRequest(HttpRequestMethod method, Uri uri, HttpRequestContent? content, HttpCompletionMode completionMode = HttpCompletionMode.ContentRead)
    {
        Guard.IsNotNull(uri);

        Method = method;
        Uri = uri;
        Content = content;
        CompletionMode = completionMode;
    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        Content?.Dispose();
    }
}
