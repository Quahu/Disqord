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
    ICurrentUser? CurrentUser { get; }

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
