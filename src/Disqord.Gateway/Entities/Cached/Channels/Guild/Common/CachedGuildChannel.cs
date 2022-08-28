using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway;

/// <inheritdoc cref="IGuildChannel"/>
public abstract class CachedGuildChannel : CachedChannel, IGuildChannel
{
    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc/>
    public virtual int Position { get; private set; }

    /// <inheritdoc/>
    public virtual IReadOnlyList<IOverwrite> Overwrites { get; private set; } = null!;

    /// <inheritdoc/>
    public GuildChannelFlags Flags { get; private set; }

    /// <inheritdoc/>
    public string Mention => Disqord.Mention.Channel(this);

    protected CachedGuildChannel(IGatewayClient client, ChannelJsonModel model)
        : base(client, model)
    {
        GuildId = model.GuildId.Value;
    }

    public override void Update(ChannelJsonModel model)
    {
        base.Update(model);

        if (!Type.IsThread())
        {
            if (model.Position.HasValue)
                Position = model.Position.Value;

            if (model.PermissionOverwrites.HasValue)
                Overwrites = model.PermissionOverwrites.Value.ToReadOnlyList(this, (x, @this) => new TransientOverwrite(@this.Client, @this.Id, x));
        }

        if (model.Flags.HasValue)
            Flags = model.Flags.Value;
    }

    public static CachedGuildChannel Create(IGatewayClient client, ChannelJsonModel model)
    {
        switch (model.Type)
        {
            case ChannelType.Text:
            case ChannelType.News:
                return new CachedTextChannel(client, model);

            case ChannelType.Voice:
                return new CachedVoiceChannel(client, model);

            case ChannelType.Category:
                return new CachedCategoryChannel(client, model);

            case ChannelType.NewsThread:
            case ChannelType.PublicThread:
            case ChannelType.PrivateThread:
                return new CachedThreadChannel(client, model);

            case ChannelType.Stage:
                return new CachedStageChannel(client, model);

            case ChannelType.Forum:
                return new CachedForumChannel(client, model);
        }

        return new CachedUnknownGuildChannel(client, model);
    }
}
