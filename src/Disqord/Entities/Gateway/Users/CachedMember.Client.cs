using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public sealed partial class CachedMember : CachedUser, IMember
    {
        public Task ModifyAsync(Action<ModifyMemberProperties> action, RestRequestOptions options = null)
            => Client.ModifyMemberAsync(Guild.Id, Id, action, options);

        public Task GrantRoleAsync(Snowflake roleId, RestRequestOptions options = null)
            => Client.GrantRoleAsync(Guild.Id, Id, roleId, options);

        public Task RevokeRoleAsync(Snowflake roleId, RestRequestOptions options = null)
            => Client.RevokeRoleAsync(Guild.Id, Id, roleId, options);

        public Task KickAsync(RestRequestOptions options = null)
            => Client.KickMemberAsync(Guild.Id, Id, options);

        public Task BanAsync(string reason = null, int? messageDeleteDays = null, RestRequestOptions options = null)
            => Client.BanMemberAsync(Guild.Id, Id, reason, messageDeleteDays, options);

        public Task UnbanAsync(RestRequestOptions options = null)
            => Client.UnbanMemberAsync(Guild.Id, Id, options);
    }
}
