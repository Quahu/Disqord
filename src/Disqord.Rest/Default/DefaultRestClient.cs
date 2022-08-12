using System.Collections.Generic;
using Disqord.Rest.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon.Collections.Synchronized;

namespace Disqord.Rest.Default;

public class DefaultRestClient : IRestClient
{
    public ILogger Logger { get; }

    public IRestApiClient ApiClient { get; }

    public IDictionary<Snowflake, IDirectChannel>? DirectChannels { get; }

    public DefaultRestClient(
        IOptions<DefaultRestClientConfiguration> options,
        ILogger<DefaultRestClient> logger,
        IRestApiClient apiClient)
    {
        var configuration = options.Value;
        if (configuration.CachesDirectChannels)
            DirectChannels = new SynchronizedDictionary<Snowflake, IDirectChannel>();

        Logger = logger;
        ApiClient = apiClient;
    }
}
