using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Gateway.Api;

namespace Disqord.Gateway
{
    public partial interface IGatewayClient : IClient
    {
        IGatewayCacheProvider CacheProvider { get; }

        IGatewayDispatcher Dispatcher { get; }

        new IGatewayApiClient ApiClient { get; }

        IApiClient IClient.ApiClient => ApiClient;

        ICurrentUser CurrentUser { get; }

        IReadOnlySet<Snowflake> UnavailableGuilds { get; }

        /// <summary>
        ///     Runs this <see cref="IGatewayClient"/>.
        /// </summary>
        /// <param name="uri"> The Discord gateway URI to connect to. </param>
        /// <param name="stoppingToken"> The token used to signal connection stopping. </param>
        /// <returns> The <see cref="Task"/> representing the connection. </returns>
        Task RunAsync(Uri uri, CancellationToken stoppingToken);
    }
}
