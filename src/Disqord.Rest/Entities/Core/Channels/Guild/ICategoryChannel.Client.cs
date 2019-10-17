using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface ICategoryChannel : IGuildChannel
    {
        Task ModifyAsync(Action<ModifyCategoryChannelProperties> action, RestRequestOptions options = null);

        Task<IReadOnlyList<RestNestedChannel>> GetChannelsAsync(RestRequestOptions options = null);
    }
}
