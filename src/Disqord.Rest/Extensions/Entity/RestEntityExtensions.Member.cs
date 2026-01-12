using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task<IGuild> FetchGuildAsync(this IMember member,
        bool? withCounts = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = member.GetRestClient();
        return client.FetchGuildAsync(member.GuildId, withCounts, options, cancellationToken)!;
    }

    public static Task<IMember> ModifyAsync(this IMember member,
        Action<ModifyMemberActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = member.GetRestClient();
        return client.ModifyMemberAsync(member.GuildId, member.Id, action, options, cancellationToken);
    }

    public static Task GrantRoleAsync(this IMember member,
        Snowflake roleId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = member.GetRestClient();
        return client.GrantRoleAsync(member.GuildId, member.Id, roleId, options, cancellationToken);
    }

    public static Task RevokeRoleAsync(this IMember member,
        Snowflake roleId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = member.GetRestClient();
        return client.RevokeRoleAsync(member.GuildId, member.Id, roleId, options, cancellationToken);
    }

    public static Task KickAsync(this IMember member,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = member.GetRestClient();
        return client.KickMemberAsync(member.GuildId, member.Id, options, cancellationToken);
    }
    
    public static Task BanAsync(this IMember member,
        string? reason = null, TimeSpan? deleteMessageDuration = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = member.GetRestClient();
        return client.CreateBanAsync(member.GuildId, member.Id, reason, deleteMessageDuration, options, cancellationToken);
    }

    [Obsolete("`delete_message_days` has been deprecated by Discord. Prefer the overload of BanAsync that accepts a `TimeSpan?`.")]
    public static Task BanAsync(this IMember member,
        string? reason = null, int? deleteMessageDays = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = member.GetRestClient();
        return client.CreateBanAsync(member.GuildId, member.Id, reason, deleteMessageDays, options, cancellationToken);
    }

    public static Task UnbanAsync(this IMember member,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = member.GetRestClient();
        return client.DeleteBanAsync(member.GuildId, member.Id, options, cancellationToken);
    }
}
