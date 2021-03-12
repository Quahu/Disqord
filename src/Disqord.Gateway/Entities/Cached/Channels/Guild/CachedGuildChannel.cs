using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Gateway
{
    public abstract class CachedGuildChannel : CachedChannel, IGuildChannel
    {
        public Snowflake GuildId { get; }

        public int Position { get; private set; }

        public IReadOnlyList<IOverwrite> Overwrites { get; private set; }

        public CachedGuildChannel(IGatewayClient client, Snowflake guildId, ChannelJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }

        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.Position.HasValue)
                Position = model.Position.Value;

            if (model.PermissionOverwrites.HasValue)
                Overwrites = model.PermissionOverwrites.Value.ToReadOnlyList(this, (x, @this) => new TransientOverwrite(@this.Client, @this.Id, x));
        }

        public static new CachedGuildChannel Create(IGatewayClient client, Snowflake guildId, ChannelJsonModel model)
        {
            switch (model.Type)
            {
                case ChannelType.Text:
                case ChannelType.News:
                case ChannelType.Store:
                // TODO: threads
                case ChannelType.Thread:
                    return new CachedTextChannel(client, guildId, model);

                case ChannelType.Voice:
                    return new CachedVoiceChannel(client, guildId, model);

                case ChannelType.Category:
                    return new CachedCategoryChannel(client, guildId, model);
            }

            return new CachedUnknownGuildChannel(client, guildId, model);
        }
    }
}
