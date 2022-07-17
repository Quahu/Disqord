using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Http;

public interface IHttpClient : IDisposable
{
    /// <summary>
    ///     Gets or sets the base url for HTTP requests.
    /// </summary>
    Uri? BaseUri { get; set; }

    /// <summary>
    ///     Sets a default header value for HTTP requests.
    /// </summary>
    /// <param name="name"> The name of the header. </param>
    /// <param name="value"> The value of the header. </param>
    void SetDefaultHeader(string name, string value);

    /// <summary>
    ///     Sends a HTTP request and returns the response.
    /// </summary>
    /// <param name="request"> The HTTP request. </param>
    /// <param name="cancellationToken"> The cancellation token. </param>
    /// <returns>
    ///     The HTTP response.
    /// </returns>
    Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken = default);
}
