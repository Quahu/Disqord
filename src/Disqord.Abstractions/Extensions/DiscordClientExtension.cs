using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Microsoft.Extensions.Logging;

namespace Disqord;

public abstract class DiscordClientExtension : ILogging
{
    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <summary>
    ///     Gets the client this extension is bound to.
    /// </summary>
    /// <remarks>
    ///     This property is set when <see cref="InitializeAsync(CancellationToken)"/> is called.
    /// </remarks>
    public DiscordClientBase Client { get; private set; } = null!;

    /// <summary>
    ///     Gets whether this extension is initialized.
    /// </summary>
    public bool IsInitialized { get; private set; }

    private protected DiscordClientExtension(
        ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    ///     Overridable logic for initialization code.
    /// </summary>
    /// <param name="cancellationToken"> The cancellation token passed from <see cref="InitializeAsync(DiscordClientBase, CancellationToken)"/>. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the initialization work.
    /// </returns>
    protected virtual ValueTask InitializeAsync(CancellationToken cancellationToken)
    {
        return default;
    }

    /// <summary>
    ///     Binds this extension to the provided client and initializes it.
    /// </summary>
    /// <param name="client"> The calling client to bind to. </param>
    /// <param name="cancellationToken"> The cancellation token. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the initialization work.
    /// </returns>
    public async Task InitializeAsync(DiscordClientBase client, CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
            throw new InvalidOperationException($"This extension ({GetType().Name}) has already been initialized.");

        Client = client;
        await InitializeAsync(cancellationToken).ConfigureAwait(false);
        IsInitialized = true;
    }
}
