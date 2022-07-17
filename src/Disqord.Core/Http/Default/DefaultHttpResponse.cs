using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Disqord.Http.Default;

/// <inheritdoc cref="IHttpResponse"/>
public class DefaultHttpResponse : HeadersBase, IHttpResponse
{
    /// <inheritdoc/>
    public HttpResponseStatusCode StatusCode => (HttpResponseStatusCode) _message.StatusCode;

    /// <inheritdoc/>
    public string? ReasonPhrase => _message.ReasonPhrase;

    private readonly HttpResponseMessage _message;

    /// <summary>
    ///     Instantiates a new <see cref="DefaultHttpResponse"/>.
    /// </summary>
    /// <param name="message"> The HTTP response message. </param>
    public DefaultHttpResponse(HttpResponseMessage message)
        : base(new HttpHeadersDictionary(message.Headers))
    {
        _message = message;
    }

    /// <inheritdoc/>
    public Task<Stream> ReadAsync()
        => _message.Content.ReadAsStreamAsync();

    /// <inheritdoc/>
    public void Dispose()
    {
        _message.Dispose();
    }
}
