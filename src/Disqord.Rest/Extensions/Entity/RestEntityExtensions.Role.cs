using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task<IRole> ModifyAsync(this IRole role,
        Action<ModifyRoleActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = role.GetRestClient();
        return client.ModifyRoleAsync(role.GuildId, role.Id, action, options, cancellationToken);
    }

    public static Task DeleteAsync(this IRole role,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = role.GetRestClient();
        return client.DeleteRoleAsync(role.GuildId, role.Id, options, cancellationToken);
    }
}