using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public sealed partial class CachedRole : CachedSnowflakeEntity, IRole
    {
        public async Task ModifyAsync(Action<ModifyRoleProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.RestClient.InternalModifyRoleAsync(Guild.Id, Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteRoleAsync(Guild.Id, Id, options);
    }
}