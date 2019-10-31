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
            Channels = new ReadOnlyValuePredicateDictionary<Snowflake, CachedNestedChannel>(guild.NestedChannels, x => x.CategoryId == Id);
            Update(model);
        }
    }
}
