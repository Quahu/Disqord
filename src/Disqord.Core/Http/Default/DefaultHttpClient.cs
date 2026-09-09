using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Http.Default;

public sealed partial class DefaultHttpClient(HttpClient http) : IHttpClient
{
    public Uri? BaseUri
    {
        get => http.BaseAddress;
        set => http.BaseAddress = value;
    }

    public async Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(request);

        using (var requestMessage = GetHttpMessage(request))
        {
            var completionOption = GetHttpCompletionOption(request.CompletionMode);
            var responseMessage = await http.SendAsync(requestMessage, completionOption, cancellationToken).ConfigureAwait(false);
            return new DefaultHttpResponse(responseMessage);
        }
    }

    public void SetDefaultHeader(string name, string value)
    {
        http.DefaultRequestHeaders.Set(name, value);
    }

    public void Dispose()
    {
        http.Dispose();
    }
}
