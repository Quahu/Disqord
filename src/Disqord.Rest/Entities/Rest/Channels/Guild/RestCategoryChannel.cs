using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestCategoryChannel : RestGuildChannel, ICategoryChannel
    {
        internal RestCategoryChannel(RestDiscordClient client, ChannelModel model) : base(client, model)
        {
            Update(model);
        }

        public async Task<IReadOnlyList<RestNestedChannel>> GetChannelsAsync(RestRequestOptions options = null)
        {
            var channels = await Client.GetChannelsAsync(GuildId, options).ConfigureAwait(false);
            return channels.OfType<RestNestedChannel>().Where(x => x.CategoryId == Id).ToImmutableArray();
        }
    }
}
