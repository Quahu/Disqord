using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public abstract partial class CachedGuildChannel : CachedChannel, IGuildChannel
    {
        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteOrCloseChannelAsync(Id, options);

        public Task AddOrModifyOverwriteAsync(LocalOverwrite overwrite, RestRequestOptions options = null)
            => Client.AddOrModifyOverwriteAsync(Id, overwrite, options);

        public Task DeleteOverwriteAsync(Snowflake targetId, RestRequestOptions options = null)
            => Client.DeleteOverwriteAsync(Id, targetId, options);
    }
}