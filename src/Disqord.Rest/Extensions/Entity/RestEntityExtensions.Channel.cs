using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<ITextChannel> ModifyAsync(this ITextChannel channel, Action<ModifyTextChannelActionProperties> action, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.ModifyTextChannelAsync(channel.Id, action, options);
        }

        public static Task<IVoiceChannel> ModifyAsync(this IVoiceChannel channel, Action<ModifyVoiceChannelActionProperties> action, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.ModifyVoiceChannelAsync(channel.Id, action, options);
        }

        public static Task<ICategoryChannel> ModifyAsync(this ICategoryChannel channel, Action<ModifyCategoryChannelActionProperties> action, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.ModifyCategoryChannelAsync(channel.Id, action, options);
        }

        public static Task DeleteAsync(this IChannel channel, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.DeleteChannelAsync(channel.Id, options);
        }

        public static IPagedEnumerable<IMessage> EnumerateMessages(this IMessageChannel channel, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.EnumerateMessages(channel.Id, limit, direction, startFromId, options);
        }

        public static Task<IReadOnlyList<IMessage>> FetchMessagesAsync(this IMessageChannel channel, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.FetchMessagesAsync(channel.Id, limit, direction, startFromId, options);
        }

        public static Task<IMessage> FetchMessageAsync(this IMessageChannel channel, Snowflake messageId, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.FetchMessageAsync(channel.Id, messageId, options);
        }

        public static Task<IUserMessage> SendMessageAsync(this IMessageChannel channel, LocalMessage message, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.SendMessageAsync(channel.Id, message, options);
        }

        public static Task<IUserMessage> CrosspostMessageAsync(this ITextChannel channel, Snowflake messageId, IRestRequestOptions options = null)
        {
            if (!channel.IsNews)
                throw new ArgumentException("This text channel must be a news channel to have messages crossposted from it.", nameof(channel));
            
            var client = channel.GetRestClient();
            return client.CrosspostMessageAsync(channel.Id, messageId, options);
        }

        public static Task AddReactionAsync(this IMessageChannel channel, Snowflake messageId, LocalEmoji emoji, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.AddReactionAsync(channel.Id, messageId, emoji, options);
        }

        public static Task RemoveOwnReactionAsync(this IMessageChannel channel, Snowflake messageId, LocalEmoji emoji, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.RemoveOwnReactionAsync(channel.Id, messageId, emoji, options);
        }

        public static Task RemoveReactionAsync(this IMessageChannel channel, Snowflake messageId, LocalEmoji emoji, Snowflake userId, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.RemoveReactionAsync(channel.Id, messageId, emoji, userId, options);
        }

        public static IPagedEnumerable<IUser> EnumerateReactions(this IMessageChannel channel, Snowflake messageId, LocalEmoji emoji, int limit, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.EnumerateReactions(channel.Id, messageId, emoji, limit, startFromId, options);
        }

        public static Task<IReadOnlyList<IUser>> FetchReactionsAsync(this IMessageChannel channel, Snowflake messageId, LocalEmoji emoji, int limit = 100, Snowflake? startFromId = null, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.FetchReactionsAsync(channel.Id, messageId, emoji, limit, startFromId, options);
        }

        public static Task ClearReactionsAsync(this IMessageChannel channel, Snowflake messageId, LocalEmoji emoji = null, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.ClearReactionsAsync(channel.Id, messageId, emoji, options);
        }

        public static Task<IUserMessage> ModifyMessageAsync(this IMessageChannel channel, Snowflake messageId, Action<ModifyMessageActionProperties> action, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.ModifyMessageAsync(channel.Id, messageId, action, options);
        }

        public static Task DeleteMessageAsync(this IMessageChannel channel, Snowflake messageId, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.DeleteMessageAsync(channel.Id, messageId, options);
        }

        public static IPagedEnumerable<Snowflake> EnumerateMessageDeletion(this ITextChannel channel, IEnumerable<Snowflake> messageIds, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.EnumerateMessageDeletion(channel.Id, messageIds, options);
        }

        public static Task DeleteMessagesAsync(this ITextChannel channel, IEnumerable<Snowflake> messageIds, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.DeleteMessagesAsync(channel.Id, messageIds, options);
        }

        public static Task SetOverwriteAsync(this IGuildChannel channel, LocalOverwrite overwrite, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.SetOverwriteAsync(channel.Id, overwrite, options);
        }

        public static Task DeleteOverwriteAsync(this IGuildChannel channel, Snowflake targetId, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.DeleteOverwriteAsync(channel.Id, targetId, options);
        }

        public static Task<IReadOnlyList<IInvite>> FetchInvitesAsync(this IGuildChannel channel, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.FetchChannelInvitesAsync(channel.Id, options);
        }

        public static Task<IInvite> CreateInviteAsync(this IGuildChannel channel, TimeSpan maxAge = default, int maxUses = 0, bool isTemporaryMembership = false, bool isUnique = false, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.CreateInviteAsync(channel.Id, maxAge, maxUses, isTemporaryMembership, isUnique, options);
        }

        public static Task<IFollowedChannel> FollowAsync(this ITextChannel channel, Snowflake targetChannelId, IRestRequestOptions options = null)
        {
            if (!channel.IsNews)
                throw new ArgumentException("This text channel must be a news channel to follow it.", nameof(channel));
            
            var client = channel.GetRestClient();
            return client.FollowNewsChannelAsync(channel.Id, targetChannelId, options);
        }

        public static Task TriggerTypingAsync(this IMessageChannel channel, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.TriggerTypingAsync(channel.Id, options);
        }

        public static IDisposable BeginTyping(this IMessageChannel channel, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.BeginTyping(channel.Id, options);
        }

        public static Task<IReadOnlyList<IUserMessage>> FetchPinnedMessagesAsync(this IMessageChannel channel, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.FetchPinnedMessagesAsync(channel.Id, options);
        }

        public static Task PinMessageAsync(this IMessageChannel channel, Snowflake messageId, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.PinMessageAsync(channel.Id, messageId, options);
        }

        public static Task UnpinMessageAsync(this IMessageChannel channel, Snowflake messageId, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.UnpinMessageAsync(channel.Id, messageId, options);
        }

        /*
         * Webhooks
         */
        public static Task<IWebhook> CreateWebhookAsync(this IMessageChannel channel, string name, Action<CreateWebhookActionProperties> action = null, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.CreateWebhookAsync(channel.Id, name, action, options);
        }

        public static Task<IReadOnlyList<IWebhook>> FetchWebhooksAsync(this IMessageChannel channel, IRestRequestOptions options = null)
        {
            var client = channel.GetRestClient();
            return client.FetchChannelWebhooksAsync(channel.Id, options);
        }
    }
}
