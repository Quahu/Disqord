using Disqord.Http;
using Qommon;

namespace Disqord.Rest.Api.Default;

public class DefaultRestResponse : IRestResponse
{
    public IHttpResponse HttpResponse { get; }

    public DefaultRestResponse(IHttpResponse httpResponse)
    {
        Guard.IsNotNull(httpResponse);

        HttpResponse = httpResponse;
    }

    public void Dispose()
    {
        HttpResponse.Dispose();
    }
}