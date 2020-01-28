using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;

namespace Disqord.Rest
{
    public sealed partial class RestCategoryChannel : RestGuildChannel, ICategoryChannel
    {
        public async Task ModifyAsync(Action<ModifyCategoryChannelProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyChannelAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public async Task<IReadOnlyList<RestNestedChannel>> GetChannelsAsync(RestRequestOptions options = null)
        {
            var channels = await Client.GetChannelsAsync(GuildId, options).ConfigureAwait(false);
            return channels.OfType<RestNestedChannel>().Where(x => x.CategoryId == Id).ToReadOnlyList();
        }
    }
}
