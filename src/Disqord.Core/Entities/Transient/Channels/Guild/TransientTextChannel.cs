using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="ITextChannel"/>
public class TransientTextChannel : TransientMessageGuildChannel, ITextChannel
{
    /// <inheritdoc/>
    public string Topic => Model.Topic.Value;

    /// <inheritdoc/>
    public bool IsAgeRestricted => Model.Nsfw.Value;

    /// <inheritdoc/>
    public bool IsNews => Model.Type == ChannelType.News;

    /// <inheritdoc/>
    public TimeSpan DefaultAutomaticArchiveDuration => TimeSpan.FromMinutes(Model.DefaultAutoArchiveDuration.GetValueOrDefault(1440));

    public TransientTextChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }
}
