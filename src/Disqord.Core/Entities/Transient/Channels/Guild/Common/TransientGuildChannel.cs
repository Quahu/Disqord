using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord;

/// <inheritdoc cref="IGuildChannel"/>
public abstract class TransientGuildChannel : TransientChannel, IGuildChannel
{
    /// <inheritdoc/>
    public Snowflake GuildId => Model.GuildId.Value;

    /// <inheritdoc/>
    public virtual int Position => Model.Position.Value;

    /// <inheritdoc/>
    public virtual IReadOnlyList<IOverwrite> Overwrites
    {
        get
        {
            if (!Model.PermissionOverwrites.HasValue)
                return ReadOnlyList<IOverwrite>.Empty;

            return _overwrites ??= Model.PermissionOverwrites.Value.ToReadOnlyList(this, (model, @this) => new TransientOverwrite(@this.Client, @this.Id, model));
        }
    }

    private IReadOnlyList<IOverwrite>? _overwrites;

    /// <inheritdoc/>
    public GuildChannelFlags Flags => Model.Flags.Value;

    /// <inheritdoc/>
    public string Mention => Disqord.Mention.Channel(this);

    protected TransientGuildChannel(IClient client, ChannelJsonModel model)
        : base(client, model)
    { }

    public new static TransientGuildChannel Create(IClient client, ChannelJsonModel model)
    {
        switch (model.Type)
        {
            case ChannelType.Text:
            case ChannelType.News:
                return new TransientTextChannel(client, model);

            case ChannelType.Voice:
                return new TransientVoiceChannel(client, model);

            case ChannelType.Category:
                return new TransientCategoryChannel(client, model);

            case ChannelType.NewsThread:
            case ChannelType.PublicThread:
            case ChannelType.PrivateThread:
                return new TransientThreadChannel(client, model);

            case ChannelType.Stage:
                return new TransientStageChannel(client, model);

            case ChannelType.Forum:
                return new TransientForumChannel(client, model);
        }

        return new TransientUnknownGuildChannel(client, model);
    }
}
