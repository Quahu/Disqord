using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public sealed partial class RestMember : RestUser, IMember
    {
        public Task ModifyAsync(Action<ModifyMemberProperties> action, RestRequestOptions options = null)
            => Client.ModifyMemberAsync(GuildId, Id, action, options);

        public Task GrantRoleAsync(Snowflake roleId, RestRequestOptions options = null)
            => Client.GrantRoleAsync(GuildId, Id, roleId, options);

        public Task RevokeRoleAsync(Snowflake roleId, RestRequestOptions options = null)
            => Client.RevokeRoleAsync(GuildId, Id, roleId, options);

        public Task KickAsync(RestRequestOptions options = null)
            => Client.KickMemberAsync(GuildId, Id, options);

        public Task BanAsync(string reason = null, int? messageDeleteDays = null, RestRequestOptions options = null)
            => Client.BanMemberAsync(GuildId, Id, reason, messageDeleteDays, options);

        public Task UnbanAsync(RestRequestOptions options = null)
            => Client.UnbanMemberAsync(GuildId, Id, options);
    }
}
