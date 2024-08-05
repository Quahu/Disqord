using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task<ICurrentUser> ModifyAsync(this ICurrentUser user,
        Action<ModifyCurrentUserActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = user.GetRestClient();
        return client.ModifyCurrentUserAsync(action, options, cancellationToken);
    }

    public static Task<IDirectChannel> CreateDirectChannelAsync(this IUser user,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = user.GetRestClient();
        return client.CreateDirectChannelAsync(user.Id, options, cancellationToken);
    }

    public static async Task<IUserMessage> SendMessageAsync(this IUser user,
        LocalMessage message,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var channel = await user.CreateDirectChannelAsync(options, cancellationToken).ConfigureAwait(false);
        return await channel.SendMessageAsync(message, options, cancellationToken).ConfigureAwait(false);
    }
}