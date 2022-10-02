using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Rest;
using Disqord.Rest.Api;
using Microsoft.Extensions.Logging;

namespace Disqord;

/// <summary>
///     Represents a high-level client for the Discord API.
/// </summary>
/// <remarks>
///     Wraps <see cref="IRestClient"/> and <see cref="IGatewayClient"/>.
/// </remarks>
public abstract partial class DiscordClientBase : IRestClient, IGatewayClient
{
    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <inheritdoc cref="IClient.ApiClient"/>
    public DiscordApiClient ApiClient { get; }

    /// <inheritdoc/>
    public IGatewayCacheProvider CacheProvider => GatewayClient.CacheProvider;

    /// <inheritdoc/>
    public IGatewayChunker Chunker => GatewayClient.Chunker;

    /// <inheritdoc/>
    public ICurrentUser CurrentUser => GatewayClient.CurrentUser;

    /// <inheritdoc/>
    public Snowflake CurrentApplicationId => GatewayClient.CurrentApplicationId;

    /// <inheritdoc/>
    public ApplicationFlags CurrentApplicationFlags => GatewayClient.CurrentApplicationFlags;

    /// <summary>
    ///     Gets the <see cref="CancellationToken"/> passed to <see cref="RunAsync(CancellationToken)"/>.
    /// </summary>
    /// <remarks>
    ///     This is set by implementations of this type.
    /// </remarks>
    /// <returns>
    ///     The cancellation token or <see cref="CancellationToken.None"/> if the client has not been started.
    /// </returns>
    public virtual CancellationToken StoppingToken { get; private protected set; }

    /// <summary>
    ///     Gets the REST client this client wraps.
    /// </summary>
    internal IRestClient RestClient { get; }

    /// <summary>
    ///     Gets the gateway client this client wraps.
    /// </summary>
    internal IGatewayClient GatewayClient { get; }

    private readonly Dictionary<Type, DiscordClientExtension> _extensions;

    IApiClient IClient.ApiClient => ApiClient;

    IGatewayApiClient IGatewayClient.ApiClient => ApiClient;

    IRestApiClient IRestClient.ApiClient => ApiClient;

    IGatewayDispatcher IGatewayClient.Dispatcher => GatewayClient.Dispatcher;

    IDictionary<Snowflake, IDirectChannel>? IRestClient.DirectChannels => RestClient.DirectChannels;

    /// <summary>
    ///     Instantiates a new <see cref="DiscordClientBase"/>, wrapping REST and gateway clients.
    /// </summary>
    /// <param name="logger"> The logger of this client. </param>
    /// <param name="restClient"> The REST client to wrap. </param>
    /// <param name="gatewayClient"> The gateway client to wrap. </param>
    /// <param name="extensions"> The extensions to use. </param>
    protected DiscordClientBase(
        ILogger logger,
        IRestClient restClient,
        IGatewayClient gatewayClient,
        IEnumerable<DiscordClientExtension> extensions)
    {
        Logger = logger;
        ApiClient = new(this);
        RestClient = restClient;
        GatewayClient = gatewayClient;
        _extensions = extensions.ToDictionary(x => x.GetType(), x => x);

        // Binds `this` to the dispatcher and coordinator, where `this` is the DiscordClientBase.
        Bind(this, this);
    }

    /// <summary>
    ///     Instantiates a new <see cref="DiscordClientBase"/>, wrapping a pre-existing client.
    /// </summary>
    /// <remarks>
    ///     Rebinds the specified client's dispatcher to <see langword="this"/>.
    /// </remarks>
    /// <param name="logger"> The logger of this client. </param>
    /// <param name="client"> The client to wrap. </param>
    private protected DiscordClientBase(
        ILogger logger,
        DiscordClientBase client)
    {
        Logger = logger;
        ApiClient = client.ApiClient;
        RestClient = client.RestClient;
        GatewayClient = client.GatewayClient;
        _extensions = client._extensions;

        // Binds `this` to the wrapped client's dispatcher and coordinator, where `this` is the client implementing DiscordBotBase.
        Bind(client, this);
    }

    private static void Bind(DiscordClientBase client, DiscordClientBase @this)
    {
        client.GatewayClient.Dispatcher.Bind(@this);

        if (client.GatewayClient.ApiClient.ShardCoordinator is DiscordShardCoordinator discordShardCoordinator)
        {
            discordShardCoordinator.Bind(@this);
        }
    }

    /// <summary>
    ///     Runs this client.
    /// </summary>
    /// <param name="stoppingToken"> The token used to signal stopping. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the work.
    /// </returns>
    public abstract Task RunAsync(CancellationToken stoppingToken);

    /// <summary>
    ///     Waits until this client is initially ready,
    ///     i.e. waits for all of its shards to be ready respecting the configured <see cref="ReadyEventDelayMode"/>.
    /// </summary>
    /// <param name="cancellationToken"> The token to observe for cancellation. </param>
    /// <returns>
    ///     A <see cref="Task"/> that completes when this client is ready.
    /// </returns>
    public abstract Task WaitUntilReadyAsync(CancellationToken cancellationToken);

    Task IGatewayClient.RunAsync(Uri? initialUri, CancellationToken stoppingToken)
    {
        return GatewayClient.RunAsync(initialUri, stoppingToken);
    }
}
