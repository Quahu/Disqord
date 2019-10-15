using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public interface ICategoryChannel : IGuildChannel
    {
        Task<IReadOnlyList<RestGuildChannel>> GetChannelsAsync(RestRequestOptions options = null);
    }
}
