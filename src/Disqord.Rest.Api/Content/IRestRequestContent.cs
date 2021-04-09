using Disqord.Http;

namespace Disqord.Rest.Api
{
    public interface IRestRequestContent
    {
        HttpRequestContent CreateHttpContent(IRestApiClient client, IRestRequestOptions options = null);
    }
}
