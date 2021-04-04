using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<IGuild> FetchGuildAsync(this IMember member, IRestRequestOptions options = null)
        {
            var client = member.GetRestClient();
            return client.FetchGuildAsync(member.GuildId, options: options);
        }

        public static Task<IMember> ModifyAsync(this IMember member, Action<ModifyMemberActionProperties> action, IRestRequestOptions options = null)
        {
            var client = member.GetRestClient();
            return client.ModifyMemberAsync(member.GuildId, member.Id, action, options);
        }

        public static Task GrantRoleAsync(this IMember member, Snowflake roleId, IRestRequestOptions options = null)
        {
            var client = member.GetRestClient();
            return client.GrantRoleAsync(member.GuildId, member.Id, roleId, options);
        }

        public static Task RevokeRoleAsync(this IMember member, Snowflake roleId, IRestRequestOptions options = null)
        {
            var client = member.GetRestClient();
            return client.RevokeRoleAsync(member.GuildId, member.Id, roleId, options);
        }

        public static Task KickAsync(this IMember member, IRestRequestOptions options = null)
        {
            var client = member.GetRestClient();
            return client.KickMemberAsync(member.GuildId, member.Id, options);
        }

        public static Task BanAsync(this IMember member, string reason = null, int? deleteMessageDays = null, IRestRequestOptions options = null)
        {
            var client = member.GetRestClient();
            return client.CreateBanAsync(member.GuildId, member.Id, reason, deleteMessageDays, options);
        }

        public static Task UnbanAsync(this IMember member, IRestRequestOptions options = null)
        {
            var client = member.GetRestClient();
            return client.DeleteBanAsync(member.GuildId, member.Id, options);
        }
    }
}
