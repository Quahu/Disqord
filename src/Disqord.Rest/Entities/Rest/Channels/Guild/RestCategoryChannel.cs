using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestCategoryChannel : RestGuildChannel, ICategoryChannel
    {
        public RestDownloadable<IReadOnlyList<RestGuildChannel>> Channels { get; }

        internal RestCategoryChannel(RestDiscordClient client, ChannelModel model, RestGuild guild = null) : base(client, model, guild)
        {
            Channels = new RestDownloadable<IReadOnlyList<RestGuildChannel>>(async options =>
                (await Client.GetChannelsAsync(GuildId, options).ConfigureAwait(false)).Where(x => x.CategoryId == Id).ToImmutableArray());
            Update(model);
        }

        public Task<IReadOnlyList<RestGuildChannel>> GetChannelsAsync(RestRequestOptions options = null)
            => Channels.DownloadAsync(options);
    }
}
