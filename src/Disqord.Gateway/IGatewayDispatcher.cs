using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Logging;
using Qommon.Binding;

namespace Disqord.Gateway;

public partial interface IGatewayDispatcher : IBindable<IGatewayClient>, ILogging
{
    /// <summary>
    ///     Gets the client this dispatcher is bound to.
    /// </summary>
    IGatewayClient Client { get; }

    /// <summary>
    ///     Gets the current user.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if accessed before <see cref="GatewayDispatchNames.Ready"/> is processed.
    /// </exception>
    ICurrentUser CurrentUser { get; }

    /// <summary>
    ///     Gets the ID of the current bot application.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if accessed before <see cref="GatewayDispatchNames.Ready"/> is processed.
    /// </exception>
    Snowflake CurrentApplicationId { get; }

    /// <summary>
    ///     Gets the flags of the current bot application.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if accessed before <see cref="GatewayDispatchNames.Ready"/> is processed.
    /// </exception>
    ApplicationFlags CurrentApplicationFlags { get; }

    /// <summary>
    ///     Waits until all the shards are initially ready, respecting the configured <see cref="ReadyEventDelayMode"/>.
    /// </summary>
    /// <param name="cancellationToken"> The token to observe for cancellation. </param>
    /// <returns>
    ///     A <see cref="Task"/> that completes when the shards are ready.
    /// </returns>
    Task WaitUntilReadyAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Handles the received dispatch and fires the matching event on the <see cref="Client"/>.
    /// </summary>
    /// <param name="sender"> The event sender. </param>
    /// <param name="e"> The dispatch event data. </param>
    /// <returns>
    ///     A task representing the work.
    /// </returns>
    ValueTask HandleDispatchAsync(object? sender, GatewayDispatchReceivedEventArgs e);
}
