using System.Collections.Generic;
using Disqord.Api;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public interface IRestClient : IClient
    {
        IDictionary<Snowflake, IDirectChannel> DirectChannels { get; }

        new IRestApiClient ApiClient { get; }

        IApiClient IClient.ApiClient => ApiClient;
    }
}
