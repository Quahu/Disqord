using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="IMessageGuildChannel"/>
public abstract class TransientMessageGuildChannel : TransientCategorizableGuildChannel, IMessageGuildChannel
{
    /// <inheritdoc/>
    public TimeSpan Slowmode => TimeSpan.FromSeconds(Model.RateLimitPerUser.GetValueOrDefault());

    /// <inheritdoc/>
    public Snowflake? LastMessageId => Model.LastMessageId.Value;

    /// <inheritdoc/>
    public DateTimeOffset? LastPinTimestamp => Model.LastPinTimestamp.GetValueOrDefault();

    /// <inheritdoc/>
    public string Tag => $"#{Name}";

    protected TransientMessageGuildChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}