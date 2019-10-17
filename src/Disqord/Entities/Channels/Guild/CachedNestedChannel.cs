using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord
{
    public abstract class CachedNestedChannel : CachedGuildChannel, IGuildChannel
    {
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

        internal CachedNestedChannel(DiscordClient client, ChannelModel model, CachedGuild guild) : base(client, model)
        { }

        internal override void Update(ChannelModel model)
        {
            if (model.ParentId.HasValue)
                CategoryId = model.ParentId.Value;

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