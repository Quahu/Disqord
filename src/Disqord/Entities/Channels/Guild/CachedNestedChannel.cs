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

        internal CachedNestedChannel(DiscordClient client, ChannelModel model, CachedGuild guild) : base(client, model, guild)
        { }

        internal override void Update(ChannelModel model)
        {
            if (model.ParentId.HasValue)
                CategoryId = model.ParentId.Value;

            base.Update(model);
        }

        public override string ToString()
            => Name;
    }
}