using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="IPrivateChannel"/>
public class TransientPrivateChannel : TransientChannel, IPrivateChannel
{
    /// <inheritdoc/>
    public Snowflake? LastMessageId => Model.LastMessageId.GetValueOrDefault();

    /// <inheritdoc/>
    public DateTimeOffset? LastPinTimestamp => Model.LastPinTimestamp.GetValueOrDefault();

    protected TransientPrivateChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}