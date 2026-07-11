using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

    public static Task<IInviteTargetUsersJobStatus> FetchTargetUsersJobStatusAsync(this IInvite invite,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = invite.GetRestClient();
        return client.FetchInviteTargetUsersJobStatusAsync(invite.Code, options, cancellationToken);
    }
}
