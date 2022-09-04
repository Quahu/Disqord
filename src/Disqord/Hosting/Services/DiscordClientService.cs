using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Qommon;

namespace Disqord.Hosting;

/// <summary>
///     Represents an <see cref="IHostedService"/> base class for Discord client services.
/// </summary>
/// <remarks>
///     If the implementation overrides <see cref="ExecuteAsync(CancellationToken)"/>
///     the service will additionally act as an improved version of <see cref="BackgroundService"/>.
/// </remarks>
public abstract partial class DiscordClientService : IHostedService, IDisposable, ILogging
{
    /// <summary>
    ///     Gets the logger of this service.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when accessed in the parameterless constructor.
    /// </exception>
    public ILogger Logger
    {
        get
        {
            if (_logger == null)
                Throw.InvalidOperationException("This property must not be accessed from the parameterless constructor.");

            return _logger;
        }
        protected set => _logger = value;
    }
    internal ILogger? _logger;

    /// <summary>
    ///     Gets the client of this service.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when accessed in the parameterless constructor.
    /// </exception>
    public DiscordClientBase Client
    {
        get
        {
            if (_client == null)
                Throw.InvalidOperationException("This property must not be accessed from the parameterless constructor.");

            return _client;
        }
        protected set => _client = value;
    }
    internal DiscordClientBase? _client;

    /// <summary>
    ///     Gets the priority of this service.
    ///     This dictates in which order should this service's event callbacks trigger.
    ///     Defaults to <c>0</c>.
    /// </summary>
    /// <remarks>
    ///     Higher value means higher priority.
    /// </remarks>
    public virtual int Priority => 0;

    /// <summary>
    ///     Gets the <see cref="Task"/> that represents the long-running work from <see cref="ExecuteAsync(CancellationToken)"/>.
    /// </summary>
    /// <remarks>
    ///     Returns <see langword="null"/> if the background operation has not started or <see cref="ExecuteAsync(CancellationToken)"/> is not overridden.
    /// </remarks>
    public virtual Task? ExecuteTask => _executeTask;

    private Task? _executeTask;
    private Cts? _cts;

    /// <summary>
    ///     Instantiates a new <see cref="DiscordClientService"/>.
    /// </summary>
    protected DiscordClientService()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="DiscordClientService"/> with the provided logger and client.
    /// </summary>
    /// <param name="logger"> The logger to use. </param>
    /// <param name="client"> The client to use. </param>
    protected DiscordClientService(
        ILogger logger,
        DiscordClientBase client)
    {
        Logger = logger;
        Client = client;
    }

    /// <summary>
    ///     This method is called when the <see cref="IHostedService"/> starts if it has been overridden in the implementing type.
    ///     The implementation should return a <see cref="Task"/> representing the long-running work.
    /// </summary>
    /// <param name="stoppingToken"> Triggered when <see cref="IHostedService.StopAsync(CancellationToken)"/> is called. </param>
    /// <returns>
    ///     A <see cref="Task"/> that represents the long-running work.
    /// </returns>
    protected virtual Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        var method = GetType().GetMethod(nameof(ExecuteAsync), BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(CancellationToken) }, null)!;
        if (method.DeclaringType == method.GetBaseDefinition().DeclaringType)
            return Task.CompletedTask;

        _cts = Cts.Linked(cancellationToken);
        var stoppingToken = _cts.Token;
        _executeTask = Task.Run(async () =>
        {
            try
            {
                await ExecuteAsync(stoppingToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException ex) when (ex.CancellationToken == stoppingToken)
            {
                // Ignore cancellation exceptions caused by the stopping token.
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while executing service {0}.", GetType().Name);
                throw;
            }
        }, cancellationToken);

        if (_executeTask.IsCompleted)
            return _executeTask;

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        if (_executeTask == null)
            return Task.CompletedTask;

        _cts?.Cancel();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
