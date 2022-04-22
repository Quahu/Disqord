using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord
{
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

                return _overwrites ??= Model.PermissionOverwrites.Value.ToReadOnlyList(this, (x, @this) => new TransientOverwrite(@this.Client, @this.Id, x));
            }
        }

        private IReadOnlyList<IOverwrite> _overwrites;

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

                case ChannelType.Store:
                    return new TransientStoreChannel(client, model);

                case ChannelType.NewsThread:
                case ChannelType.PublicThread:
                case ChannelType.PrivateThread:
                    return new TransientThreadChannel(client, model);

                case ChannelType.Stage:
                    return new TransientStageChannel(client, model);
            }

            return new TransientUnknownGuildChannel(client, model);
        }
    }
}
