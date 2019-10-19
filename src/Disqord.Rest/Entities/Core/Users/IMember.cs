using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord
{
    public interface IMember : IUser
    {
        string Nick { get; }

        IReadOnlyCollection<Snowflake> RoleIds { get; }

        DateTimeOffset JoinedAt { get; }

        bool IsMuted { get; }

        bool IsDeafened { get; }

        Snowflake GuildId { get; }

        DateTimeOffset? BoostedAt { get; }

        bool IsBoosting { get; }

        Task ModifyAsync(Action<ModifyMemberProperties> action, RestRequestOptions options = null);

        Task KickAsync(RestRequestOptions options = null);

        Task BanAsync(string reason = null, int? messageDeleteDays = null, RestRequestOptions options = null);

        Task UnbanAsync(RestRequestOptions options = null);

        Task GrantRoleAsync(Snowflake roleId, RestRequestOptions options = null);

        Task RevokeRoleAsync(Snowflake roleId, RestRequestOptions options = null);
    }
}
