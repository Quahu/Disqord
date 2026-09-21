using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task DeleteAsync(this IInvite invite,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = invite.GetRestClient();
        return client.DeleteInviteAsync(invite.Code, options, cancellationToken);
    }

    public static Task<IReadOnlyList<Snowflake>> FetchTargetUsersAsync(this IInvite invite,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = invite.GetRestClient();
        return client.FetchInviteTargetUsersAsync(invite.Code, options, cancellationToken);
    }

    public static Task SetTargetUsersAsync(this IInvite invite,
        IEnumerable<Snowflake> targetUsers,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = invite.GetRestClient();
        return client.UpdateInviteTargetUsersAsync(invite.Code, targetUsers, options, cancellationToken);
    }

    public static Task AddTargetUserAsync(this IInvite invite,
        Snowflake userId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = invite.GetRestClient();
        return client.AddInviteTargetUserAsync(invite.Code, userId, options, cancellationToken);
    }

    public static Task RemoveTargetUserAsync(this IInvite invite,
        Snowflake userId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = invite.GetRestClient();
        return client.RemoveInviteTargetUserAsync(invite.Code, userId, options, cancellationToken);
    }

    public static IPagedEnumerable<Snowflake> EnumerateTargetUserAddition(this IInvite invite,
        IEnumerable<Snowflake> userIds,
        IRestRequestOptions? options = null)
    {
        var client = invite.GetRestClient();
        return client.EnumerateInviteTargetUserAddition(invite.Code, userIds, options);
    }

    public static Task AddTargetUsersAsync(this IInvite invite,
        IEnumerable<Snowflake> userIds,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = invite.GetRestClient();
        return client.AddInviteTargetUsersAsync(invite.Code, userIds, options, cancellationToken);
    }

    public static IPagedEnumerable<Snowflake> EnumerateTargetUserRemoval(this IInvite invite,
        IEnumerable<Snowflake> userIds,
        IRestRequestOptions? options = null)
    {
        var client = invite.GetRestClient();
        return client.EnumerateInviteTargetUserRemoval(invite.Code, userIds, options);
    }

    public static Task RemoveTargetUsersAsync(this IInvite invite,
        IEnumerable<Snowflake> userIds,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = invite.GetRestClient();
        return client.RemoveInviteTargetUsersAsync(invite.Code, userIds, options, cancellationToken);
    }

    public static Task<IInviteTargetUsersJobStatus> FetchTargetUsersJobStatusAsync(this IInvite invite,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = invite.GetRestClient();
        return client.FetchInviteTargetUsersJobStatusAsync(invite.Code, options, cancellationToken);
    }
}
