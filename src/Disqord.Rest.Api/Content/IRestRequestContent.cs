using Disqord.Http;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public interface IRestRequestContent
    {
        HttpRequestContent CreateHttpContent(IJsonSerializer serializer, IRestRequestOptions options = null);
    }
}
