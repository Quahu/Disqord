using Disqord.Rest;
using Disqord.Rest.Api;
using Microsoft.Extensions.Logging;

namespace Disqord.Tests.Fakes;

internal sealed class FakeRestClient : IRestClient
{
    public IRestApiClient ApiClient { get; }

    public IDictionary<Snowflake, IDirectChannel>? DirectChannels => null;

    public ILogger Logger => throw new NotSupportedException();

    public FakeRestClient(IRestApiClient apiClient)
    {
        ApiClient = apiClient;
    }
}
