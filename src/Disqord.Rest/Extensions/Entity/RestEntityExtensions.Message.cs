using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static async Task<IMessageChannel?> FetchChannelAsync(this IMessage message,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = message.GetRestClient();
        return await client.FetchChannelAsync(message.ChannelId, options, cancellationToken).ConfigureAwait(false) as IMessageChannel;
    }

    public static Task AddReactionAsync(this IMessage message,
        LocalEmoji emoji,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = message.GetRestClient();
        return client.AddReactionAsync(message.ChannelId, message.Id, emoji, options, cancellationToken);
    }

    public static Task RemoveOwnReactionAsync(this IMessage message,
        LocalEmoji emoji,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = message.GetRestClient();
        return client.RemoveOwnReactionAsync(message.ChannelId, message.Id, emoji, options, cancellationToken);
    }

    public static Task RemoveReactionAsync(this IMessage message,
        LocalEmoji emoji, Snowflake userId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = message.GetRestClient();
        return client.RemoveReactionAsync(message.ChannelId, message.Id, emoji, userId, options, cancellationToken);
    }

    public static IPagedEnumerable<IUser> EnumerateReactions(this IMessage message,
        LocalEmoji emoji, int limit, Snowflake? startFromId = null,
        IRestRequestOptions? options = null)
    {
        var client = message.GetRestClient();
        return client.EnumerateReactions(message.ChannelId, message.Id, emoji, limit, startFromId, options);
    }

    public static Task<IReadOnlyList<IUser>> FetchReactionsAsync(this IMessage message,
        LocalEmoji emoji, int limit = Discord.Limits.Rest.FetchReactionsPageSize, Snowflake? startFromId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = message.GetRestClient();
        return client.FetchReactionsAsync(message.ChannelId, message.Id, emoji, limit, startFromId, options, cancellationToken);
    }

    public static Task ClearReactionsAsync(this IMessage message,
        LocalEmoji? emoji = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = message.GetRestClient();
        return client.ClearReactionsAsync(message.ChannelId, message.Id, emoji, options, cancellationToken);
    }

    public static Task<IUserMessage> ModifyAsync(this IUserMessage message,
        Action<ModifyMessageActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = message.GetRestClient();
        return client.ModifyMessageAsync(message.ChannelId, message.Id, action, options, cancellationToken);
    }

    public static Task DeleteAsync(this IMessage message,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = message.GetRestClient();
        return client.DeleteMessageAsync(message.ChannelId, message.Id, options, cancellationToken);
    }

    public static Task PinAsync(this IUserMessage message,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = message.GetRestClient();
        return client.PinMessageAsync(message.ChannelId, message.Id, options, cancellationToken);
    }

    public static Task UnpinAsync(this IUserMessage message,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = message.GetRestClient();
        return client.UnpinMessageAsync(message.ChannelId, message.Id, options, cancellationToken);
    }
}
