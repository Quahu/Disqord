using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;
using Disqord.Rest;

namespace Disqord
{
    public sealed class CachedCategoryChannel : CachedGuildChannel, ICategoryChannel
    {
        public IReadOnlyDictionary<Snowflake, CachedGuildChannel> Channels { get; }

        internal CachedCategoryChannel(DiscordClient client, ChannelModel model, CachedGuild guild) : base(client, model, guild)
        {
            Channels = new ReadOnlyValuePredicateDictionary<Snowflake, CachedGuildChannel>(guild._channels, x => x.CategoryId == Id);
            Update(model);
        }

        public async Task<IReadOnlyList<RestNestedChannel>> GetChannelsAsync(RestRequestOptions options = null)
        {
            var channels = await Client.GetChannelsAsync(Guild.Id, options).ConfigureAwait(false);
            return channels.OfType<RestNestedChannel>().Where(x => x.CategoryId == Id).ToImmutableArray();
        }
    }
}
