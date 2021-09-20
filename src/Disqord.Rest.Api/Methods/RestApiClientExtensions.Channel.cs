using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<ChannelJsonModel> FetchChannelAsync(this IRestApiClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.GetChannel, channelId);
            return client.ExecuteAsync<ChannelJsonModel>(route, null, options);
        }

        public static Task<ChannelJsonModel> ModifyChannelAsync(this IRestApiClient client, Snowflake channelId, ModifyChannelJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.ModifyChannel, channelId);
            return client.ExecuteAsync<ChannelJsonModel>(route, content, options);
        }

        public static Task<ChannelJsonModel> DeleteChannelAsync(this IRestApiClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.DeleteChannel, channelId);
            return client.ExecuteAsync<ChannelJsonModel>(route, null, options);
        }

        public static Task<MessageJsonModel[]> FetchMessagesAsync(this IRestApiClient client, Snowflake channelId, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));

            var queryParameters = new Dictionary<string, object>(startFromId != null ? 2 : 1)
            {
                ["limit"] = limit
            };

            switch (direction)
            {
                case RetrievalDirection.Around:
                {
                    queryParameters["around"] = startFromId ?? throw new ArgumentNullException(nameof(startFromId), "The ID to fetch messages around must not be null.");
                    break;
                }
                case RetrievalDirection.Before:
                {
                    if (startFromId != null)
                        queryParameters["before"] = startFromId;

                    break;
                }
                case RetrievalDirection.After:
                {
                    queryParameters["after"] = startFromId ?? throw new ArgumentNullException(nameof(startFromId), "The ID to fetch messages after must not be null.");
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(direction), "Invalid message fetch direction.");
                }
            }

            var route = Format(Route.Channel.GetMessages, queryParameters, channelId);
            return client.ExecuteAsync<MessageJsonModel[]>(route, null, options);
        }

        public static Task<MessageJsonModel> FetchMessageAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.GetMessage, channelId, messageId);
            return client.ExecuteAsync<MessageJsonModel>(route, null, options);
        }

        public static Task<MessageJsonModel> CreateMessageAsync(this IRestApiClient client, Snowflake channelId, CreateMessageJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.CreateMessage, channelId);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> CreateMessageAsync(this IRestApiClient client, Snowflake channelId, MultipartJsonPayloadRestRequestContent<CreateMessageJsonRestRequestContent> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.CreateMessage, channelId);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> CrosspostMessageAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.CrosspostMessage, channelId, messageId);
            return client.ExecuteAsync<MessageJsonModel>(route, null, options);
        }

        public static Task AddReactionAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, string emoji, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.CreateReaction, channelId, messageId, emoji);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task RemoveOwnReactionAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, string emoji, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.DeleteOwnReaction, channelId, messageId, emoji);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task RemoveUserReactionAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, string emoji, Snowflake userId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.DeleteUserReaction, channelId, messageId, emoji, userId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<UserJsonModel[]> FetchReactionsAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, string emoji, int limit = 100, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));

            var queryParameters = new Dictionary<string, object>(startFromId != null ? 2 : 1)
            {
                ["limit"] = limit
            };

            if (startFromId != null)
                queryParameters["after"] = startFromId;

            var route = Format(Route.Channel.GetReactions, queryParameters, channelId, messageId, emoji);
            return client.ExecuteAsync<UserJsonModel[]>(route, null, options);
        }

        public static Task ClearReactionsAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.ClearReactions, channelId, messageId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task ClearEmojiReactionsAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, string emoji, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.ClearEmojiReactions, channelId, messageId, emoji);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<MessageJsonModel> ModifyMessageAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, ModifyMessageJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.ModifyMessage, channelId, messageId);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task DeleteMessageAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.DeleteMessage, channelId, messageId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task DeleteMessagesAsync(this IRestApiClient client, Snowflake channelId, DeleteMessagesJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.DeleteMessages, channelId);
            return client.ExecuteAsync(route, content, options);
        }

        public static Task SetOverwriteAsync(this IRestApiClient client, Snowflake channelId, Snowflake overwriteId, SetOverwriteJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.SetOverwrite, channelId, overwriteId);
            return client.ExecuteAsync(route, content, options);
        }

        public static Task DeleteOverwriteAsync(this IRestApiClient client, Snowflake channelId, Snowflake overwriteId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.DeleteOverwrite, channelId, overwriteId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<InviteJsonModel[]> FetchChannelInvitesAsync(this IRestApiClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.GetInvites, channelId);
            return client.ExecuteAsync<InviteJsonModel[]>(route, null, options);
        }

        public static Task<InviteJsonModel> CreateChannelInviteAsync(this IRestApiClient client, Snowflake channelId, CreateChannelInviteJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.CreateInvite, channelId);
            return client.ExecuteAsync<InviteJsonModel>(route, content, options);
        }

        public static Task<FollowedChannelJsonModel> FollowNewsChannelAsync(this IRestApiClient client, Snowflake channelId, FollowNewsChannelJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.FollowNewsChannel, channelId);
            return client.ExecuteAsync<FollowedChannelJsonModel>(route, content, options);
        }

        public static Task TriggerTypingAsync(this IRestApiClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.TriggerTyping, channelId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<MessageJsonModel[]> FetchPinnedMessagesAsync(this IRestApiClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.GetPinnedMessages, channelId);
            return client.ExecuteAsync<MessageJsonModel[]>(route, null, options);
        }

        public static Task PinMessageAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.PinMessage, channelId, messageId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task UnpinMessageAsync(this IRestApiClient client, Snowflake channelId, Snowflake messageId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.UnpinMessage, channelId, messageId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<ChannelJsonModel> CreateThreadAsync(this IRestApiClient client, Snowflake channelId, CreateThreadJsonRestRequestContent content, Snowflake? messageId = null, IRestRequestOptions options = null)
        {
            var route = messageId == null
                ? Format(Route.Channel.StartThread, channelId)
                : Format(Route.Channel.StartThreadWithMessage, channelId, messageId);

            return client.ExecuteAsync<ChannelJsonModel>(route, content, options);
        }

        public static Task JoinThreadAsync(this IRestApiClient client, Snowflake threadId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.JoinThread, threadId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task AddThreadMemberAsync(this IRestApiClient client, Snowflake threadId, Snowflake memberId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.AddThreadMember, threadId, memberId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task LeaveThreadAsync(this IRestApiClient client, Snowflake threadId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.LeaveThread, threadId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task RemoveThreadMemberAsync(this IRestApiClient client, Snowflake threadId, Snowflake memberId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.RemoveThreadMember, threadId, memberId);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<ThreadMemberJsonModel[]> FetchThreadMembersAsync(this IRestApiClient client, Snowflake threadId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Channel.ListThreadMembers, threadId);
            return client.ExecuteAsync<ThreadMemberJsonModel[]>(route, null, options);
        }

        public static Task<ThreadListJsonModel> FetchPublicArchivedThreadsAsync(this IRestApiClient client, Snowflake channelId, int limit = 100, DateTimeOffset? startFromDate = null, IRestRequestOptions options = null)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));

            var queryParameters = new Dictionary<string, object>
            {
                ["limit"] = limit
            };

            if (startFromDate != null)
                queryParameters.Add("before", startFromDate.Value.ToString("O"));

            var route = Format(Route.Channel.ListPublicArchivedThreads, queryParameters, channelId);
            return client.ExecuteAsync<ThreadListJsonModel>(route, null, options);
        }

        public static Task<ThreadListJsonModel> FetchPrivateArchivedThreadsAsync(this IRestApiClient client, Snowflake channelId, int limit = 100, DateTimeOffset? startFromDate = null, IRestRequestOptions options = null)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));

            var queryParameters = new Dictionary<string, object>
            {
                ["limit"] = limit
            };

            if (startFromDate != null)
                queryParameters["before"] = startFromDate.Value.ToString("O");

            var route = Format(Route.Channel.ListPrivateArchivedThreads, queryParameters, channelId);
            return client.ExecuteAsync<ThreadListJsonModel>(route, null, options);
        }

        public static Task<ThreadListJsonModel> FetchJoinedPrivateArchivedThreadsAsync(this IRestApiClient client, Snowflake channelId, int limit = 100, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentOutOfRangeException(nameof(limit));

            var queryParameters = new Dictionary<string, object>
            {
                ["limit"] = limit
            };

            if (startFromId != null)
                queryParameters["before"] = startFromId.Value;

            var route = Format(Route.Channel.ListJoinedPrivateArchivedThreads, queryParameters, channelId);
            return client.ExecuteAsync<ThreadListJsonModel>(route, null, options);
        }
    }
}
