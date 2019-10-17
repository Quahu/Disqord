using System.Threading.Tasks;

namespace Disqord.Rest
{
    public abstract partial class RestGuildChannel : RestChannel, IGuildChannel
    {
        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteOrCloseChannelAsync(Id);

        public Task AddOrModifyOverwriteAsync(LocalOverwrite overwrite, RestRequestOptions options = null)
            => Client.AddOrModifyOverwriteAsync(Id, overwrite, options);

        public Task DeleteOverwriteAsync(Snowflake targetId, RestRequestOptions options = null)
            => Client.DeleteOverwriteAsync(Id, targetId, options);
    }
}