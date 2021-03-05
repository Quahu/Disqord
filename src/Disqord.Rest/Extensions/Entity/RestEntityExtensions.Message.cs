using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;

namespace Disqord.Rest
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class RestEntityExtensions
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IRestClient GetRestClient(this IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Client is not IRestClient client)
                throw new InvalidOperationException("This entity's client is not a REST client implementation.");

            return client;
        }

        public static Task<IChannel> FetchChannelAsync(this IMessage message, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.FetchChannelAsync(message.ChannelId, options);
        }

        // TODO: crosspost message

        public static Task AddReactionAsync(this IMessage message, IEmoji emoji, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.AddReactionAsync(message.ChannelId, message.Id, emoji, options);
        }

        public static Task RemoveOwnReactionAsync(this IMessage message, IEmoji emoji, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.RemoveOwnReactionAsync(message.ChannelId, message.Id, emoji, options);
        }

        public static Task RemoveReactionAsync(this IMessage message, IEmoji emoji, Snowflake userId, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.RemoveReactionAsync(message.ChannelId, message.Id, emoji, userId, options);
        }

        public static IPagedEnumerable<IUser> EnumerateReactions(this IMessage message, IEmoji emoji, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.EnumerateReactions(message.ChannelId, message.Id, emoji, limit, direction, startFromId, options);
        }

        public static Task<IReadOnlyList<IUser>> FetchReactionsAsync(this IMessage message, IEmoji emoji, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.FetchReactionsAsync(message.ChannelId, message.Id, emoji, limit, direction, startFromId, options);
        }

        public static Task ClearReactionsAsync(this IMessage message, IEmoji emoji = null, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.ClearReactionsAsync(message.ChannelId, message.Id, emoji, options);
        }

        public static Task<IUserMessage> ModifyAsync(this IUserMessage message, Action<ModifyMessageActionProperties> action, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.ModifyMessageAsync(message.ChannelId, message.Id, action, options);
        }

        public static Task DeleteAsync(this IMessage message, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.DeleteMessageAsync(message.ChannelId, message.Id, options);
        }

        public static Task PinAsync(this IUserMessage message, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.PinMessageAsync(message.ChannelId, message.Id, options);
        }

        public static Task UnpinAsync(this IUserMessage message, IRestRequestOptions options = null)
        {
            var client = message.GetRestClient();
            return client.UnpinMessageAsync(message.ChannelId, message.Id, options);
        }
    }
}
