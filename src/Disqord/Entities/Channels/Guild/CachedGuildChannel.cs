using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord
{
    public abstract class CachedGuildChannel : CachedChannel, IGuildChannel
    {
        public int Position { get; private set; }

        public CachedGuild Guild { get; }

        public CachedCategoryChannel Category
        {
            get
            {
                var categoryId = CategoryId;
                return categoryId != null
                    ? Guild.GetCategoryChannel(categoryId.Value)
                    : null;
            }
        }

        public Snowflake? CategoryId { get; private set; }

        public IReadOnlyList<CachedOverwrite> Overwrites { get; private set; }

        IReadOnlyList<IOverwrite> IGuildChannel.Overwrites => Overwrites;
        Snowflake IGuildChannel.GuildId => Guild.Id;

        internal CachedGuildChannel(DiscordClient client, ChannelModel model, CachedGuild guild) : base(client, model)
        {
            Guild = guild;
        }

        internal override void Update(ChannelModel model)
        {
            if (model.Position.HasValue)
                Position = model.Position.Value;

            if (model.ParentId.HasValue)
                CategoryId = model.ParentId.Value;

            if (model.PermissionOverwrites.HasValue)
                Overwrites = model.PermissionOverwrites.Value.Select(x => new CachedOverwrite(Client, x, this)).ToImmutableArray();

            base.Update(model);
        }

        internal static CachedGuildChannel Create(DiscordClient client, ChannelModel model, CachedGuild guild)
        {
            switch (model.Type.Value)
            {
                case ChannelType.Text:
                case ChannelType.News:
                    return new CachedTextChannel(client, model, guild);

                case ChannelType.Voice:
                    return new CachedVoiceChannel(client, model, guild);

                case ChannelType.Category:
                    return new CachedCategoryChannel(client, model, guild);

                default:
                    return null;
            }
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.RestClient.DeleteOrCloseChannelAsync(Id, options);

        public override string ToString()
            => Name;
    }
}