﻿using System.Collections.Generic;
using Disqord.Collections.Synchronized;
using Disqord.Rest.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Rest.Default
{
    public class DefaultRestClient : IRestClient
    {
        public ILogger Logger { get; }

        public IRestApiClient ApiClient { get; }

        public IDictionary<Snowflake, IDirectChannel> DirectChannels { get; }

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

        public void Dispose()
        {
            ApiClient.Dispose();
        }
    }
}
