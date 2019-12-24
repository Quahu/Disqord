using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public sealed partial class CachedCategoryChannel : CachedGuildChannel, ICategoryChannel
    {
        public IReadOnlyDictionary<Snowflake, CachedNestedChannel> Channels { get; }

        internal CachedCategoryChannel(CachedGuild guild, ChannelModel model) : base(guild, model)
        {
            Channels = new ReadOnlyValuePredicateArgumentDictionary<Snowflake, CachedNestedChannel, Snowflake>(
                guild.NestedChannels, (x, id) => x.CategoryId == id, Id);
            Update(model);
        }
    }
}
