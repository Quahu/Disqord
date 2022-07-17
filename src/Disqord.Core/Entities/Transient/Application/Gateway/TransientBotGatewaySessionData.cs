using System;
using Disqord.Models;

namespace Disqord.Gateway;

/// <inheritdoc cref="IBotGatewaySessionData"/>/>
public class TransientBotGatewaySessionData : TransientClientEntity<SessionStartLimitJsonModel>, IBotGatewaySessionData
{
    /// <inheritdoc/>
    public int MaxCount => Model.Total;

    /// <inheritdoc/>
    public int RemainingCount => Model.Remaining;

    /// <inheritdoc/>
    public TimeSpan ResetAfter => TimeSpan.FromMilliseconds(Model.ResetAfter);

    /// <inheritdoc/>
    public int MaxConcurrency => Model.MaxConcurrency;

    public TransientBotGatewaySessionData(IClient client, SessionStartLimitJsonModel model)
        : base(client, model)
    { }
}