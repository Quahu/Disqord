using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public abstract class RestGuildChannel : RestChannel, IGuildChannel
    {
        public int Position { get; private set; }

        public Snowflake GuildId { get; }

        public RestDownloadable<RestGuild> Guild { get; }

        public Snowflake? CategoryId { get; private set; }

        public IReadOnlyList<RestOverwrite> Overwrites { get; private set; }

        IReadOnlyList<IOverwrite> IGuildChannel.Overwrites => Overwrites;

        internal RestGuildChannel(RestDiscordClient client, ChannelModel model, RestGuild guild = null) : base(client, model)
        {
            GuildId = model.GuildId.Value;
            Guild = new RestDownloadable<RestGuild>(options => Client.GetGuildAsync(GuildId, options));
            if (guild != null)
                Guild.SetValue(guild);
        }

        internal override void Update(ChannelModel model)
        {
            if (model.Position.HasValue)
                Position = model.Position.Value;

            if (model.ParentId.HasValue)
                CategoryId = model.ParentId.Value;

            if (model.PermissionOverwrites.HasValue)
                Overwrites = model.PermissionOverwrites.Value.Select(x => new RestOverwrite(Client, x, this)).ToImmutableArray();

            base.Update(model);
        }

        internal static new RestGuildChannel Create(RestDiscordClient client, ChannelModel model)
        {
            switch (model.Type.Value)
            {
                case ChannelType.Text:
                case ChannelType.News:
                    return new RestTextChannel(client, model);

                case ChannelType.Voice:
                    return new RestVoiceChannel(client, model);

                case ChannelType.Category:
                    return new RestCategoryChannel(client, model);

                default:
                    return null;
            }
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteOrCloseChannelAsync(Id);
    }
}