using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IMember : IUser
    {
        Task ModifyAsync(Action<ModifyMemberProperties> action, RestRequestOptions options = null);

        Task GrantRoleAsync(Snowflake roleId, RestRequestOptions options = null);

        Task RevokeRoleAsync(Snowflake roleId, RestRequestOptions options = null);

        Task KickAsync(RestRequestOptions options = null);

        Task BanAsync(string reason = null, int? messageDeleteDays = null, RestRequestOptions options = null);

        Task UnbanAsync(RestRequestOptions options = null);
    }
}
