using Disqord.Models;

namespace Disqord
{
    public abstract class CachedNestedChannel : CachedGuildChannel, IGuildChannel
    {
        public Snowflake? CategoryId { get; private set; }

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

        internal CachedNestedChannel(CachedGuild guild, ChannelModel model) : base(guild, model)
        { }

        internal override void Update(ChannelModel model)
        {
            if (model.ParentId.HasValue)
                CategoryId = model.ParentId.Value;

            base.Update(model);
        }
    }
}