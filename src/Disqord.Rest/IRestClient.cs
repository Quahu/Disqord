using Disqord.Api;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public interface IRestClient : IClient
    {
        new IRestApiClient ApiClient { get; }

        IApiClient IClient.ApiClient => ApiClient;
    }
}
