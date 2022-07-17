using System;
using System.ComponentModel;
using Disqord.Gateway.Api.Models;
using Disqord.Models;
using Qommon;

namespace Disqord.Gateway;

/// <inheritdoc cref="IMessageGuildChannel"/>
public abstract class CachedMessageGuildChannel : CachedCategorizableGuildChannel, IMessageGuildChannel, IJsonUpdatable<ChannelPinsUpdateJsonModel>
{
    /// <inheritdoc/>
    public TimeSpan Slowmode { get; private set; }

    /// <inheritdoc/>
    // TODO: proper update instead of a public setter?
    public Snowflake? LastMessageId { get; set; }

    /// <inheritdoc/>
    public DateTimeOffset? LastPinTimestamp { get; private set; }

    /// <inheritdoc/>
    public string Tag => $"#{Name}";

    protected CachedMessageGuildChannel(IGatewayClient client, ChannelJsonModel model)
        : base(client, model)
    { }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void Update(ChannelJsonModel model)
    {
        base.Update(model);

        if (model.RateLimitPerUser.HasValue)
            Slowmode = TimeSpan.FromSeconds(model.RateLimitPerUser.Value);

        if (model.LastMessageId.HasValue)
            LastMessageId = model.LastMessageId.Value;

        if (model.LastPinTimestamp.HasValue)
            LastPinTimestamp = model.LastPinTimestamp.Value;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Update(ChannelPinsUpdateJsonModel model)
    {
        LastPinTimestamp = model.LastPinTimestamp.GetValueOrDefault();
    }
}