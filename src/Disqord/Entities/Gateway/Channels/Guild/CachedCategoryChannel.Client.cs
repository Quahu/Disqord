using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Rest;

namespace Disqord
{
    public sealed partial class CachedCategoryChannel : CachedGuildChannel, ICategoryChannel
    {
        public async Task ModifyAsync(Action<ModifyCategoryChannelProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.RestClient.InternalModifyChannelAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public async Task<IReadOnlyList<RestNestedChannel>> GetChannelsAsync(RestRequestOptions options = null)
        {
            var channels = await Client.GetChannelsAsync(Guild.Id, options).ConfigureAwait(false);
            return channels.OfType<RestNestedChannel>().Where(x => x.CategoryId == Id).ToReadOnlyList();
        }
    }
}
