using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public abstract partial class RestGuildChannel : RestChannel, IGuildChannel
    {
        public int Position { get; private set; }

        public Snowflake GuildId { get; }

        public RestFetchable<RestGuild> Guild { get; }

        public IReadOnlyList<RestOverwrite> Overwrites { get; private set; }

        IReadOnlyList<IOverwrite> IGuildChannel.Overwrites => Overwrites;

        internal RestGuildChannel(RestDiscordClient client, ChannelModel model) : base(client, model)
        {
            GuildId = model.GuildId.Value;
            Guild = RestFetchable.Create(this, (@this, options) =>
                @this.Client.GetGuildAsync(@this.GuildId, options));
        }

        internal override void Update(ChannelModel model)
        {
            if (model.Position.HasValue)
                Position = model.Position.Value;

            if (model.PermissionOverwrites.HasValue)
                Overwrites = model.PermissionOverwrites.Value.ToReadOnlyList(this, (x, @this) =>
                {
                    var overwrite = new RestOverwrite(@this.Client, @this.Id, x);
                    overwrite.Channel.Value = @this;
                    return overwrite;
                });

            base.Update(model);
        }

        internal static new RestGuildChannel Create(RestDiscordClient client, ChannelModel model)
        {
            switch (model.Type.Value)
            {
                case ChannelType.Text:
                case ChannelType.News:
                case ChannelType.Store:
                    return new RestTextChannel(client, model);

                case ChannelType.Voice:
                    return new RestVoiceChannel(client, model);

                case ChannelType.Category:
                    return new RestCategoryChannel(client, model);

                default:
                    return new RestUnknownGuildChannel(client, model);
            }
        }
    }
}