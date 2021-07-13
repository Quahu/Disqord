﻿using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public abstract class TransientGuildChannel : TransientChannel, IGuildChannel
    {
        public Snowflake GuildId => Model.GuildId.Value;

        public virtual int Position => Model.Position.Value;

        public virtual IReadOnlyList<IOverwrite> Overwrites => _overwrites ??= Model.PermissionOverwrites.Value.ToReadOnlyList(this, (x, @this) => new TransientOverwrite(@this.Client, @this.Id, x));
        private IReadOnlyList<IOverwrite> _overwrites;

        protected TransientGuildChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }

        public static new TransientGuildChannel Create(IClient client, ChannelJsonModel model)
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
