using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Http.Default;

public sealed partial class DefaultHttpClient : IHttpClient
{
    public Uri? BaseUri
    {
        get => _http.BaseAddress;
        set => _http.BaseAddress = value;
    }

    private readonly HttpClient _http;

    public DefaultHttpClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(request);

        using (var requestMessage = GetHttpMessage(request))
        {
            var responseMessage = await _http.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            return new DefaultHttpResponse(responseMessage);
        }
    }

    public void SetDefaultHeader(string name, string value)
    {
        _http.DefaultRequestHeaders.Set(name, value);
    }

    public void Dispose()
    {
        _http.Dispose();
    }
}
