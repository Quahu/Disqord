using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public sealed partial class RestRole : RestSnowflakeEntity, IRole
    {
        public async Task ModifyAsync(Action<ModifyRoleProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyRoleAsync(GuildId, Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteRoleAsync(GuildId, Id, options);
    }
}