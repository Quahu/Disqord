using System;
using System.ComponentModel;
using Disqord.Models;
using Qommon;

namespace Disqord.Gateway;

/// <inheritdoc cref="ITextChannel"/>
public class CachedTextChannel : CachedMessageGuildChannel, ITextChannel
{
    /// <inheritdoc/>
    public string? Topic { get; private set; }

    /// <inheritdoc/>
    public bool IsAgeRestricted { get; private set; }

    /// <inheritdoc/>
    public bool IsNews => Type == ChannelType.News;

    /// <inheritdoc/>
    public TimeSpan DefaultAutomaticArchiveDuration { get; private set; }

    public CachedTextChannel(IGatewayClient client, ChannelJsonModel model)
        : base(client, model)
    { }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void Update(ChannelJsonModel model)
    {
        base.Update(model);

        if (model.Topic.HasValue)
            Topic = model.Topic.Value;

        if (model.Nsfw.HasValue)
            IsAgeRestricted = model.Nsfw.Value;

        DefaultAutomaticArchiveDuration = TimeSpan.FromMinutes(model.DefaultAutoArchiveDuration.GetValueOrDefault(1440));
    }
}
