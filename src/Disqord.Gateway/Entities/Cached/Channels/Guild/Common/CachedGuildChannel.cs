using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Enums.Extensions;
using Disqord.Models;

namespace Disqord.Gateway
{
    public abstract class CachedGuildChannel : CachedChannel, IGuildChannel
    {
        public Snowflake GuildId { get; }

        public virtual int Position { get; private set; }

        public virtual IReadOnlyList<IOverwrite> Overwrites { get; private set; }

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
        }

        public static CachedGuildChannel Create(IGatewayClient client, ChannelJsonModel model)
        {
            switch (model.Type)
            {
                case ChannelType.Text:
                    return new CachedTextChannel(client, model);

                case ChannelType.News:
                    return new CachedNewsChannel(client, model);
                
                case ChannelType.Voice:
                    return new CachedVoiceChannel(client, model);

                case ChannelType.Category:
                    return new CachedCategoryChannel(client, model);

                case ChannelType.Store:
                    return new CachedStoreChannel(client, model);

                case ChannelType.NewsThread:
                case ChannelType.PublicThread:
                case ChannelType.PrivateThread:
                    return new CachedThreadChannel(client, model);

                case ChannelType.Stage:
                    return new CachedStageChannel(client, model);
            }

            return new CachedUnknownGuildChannel(client, model);
        }
    }
}
