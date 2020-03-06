using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public sealed partial class RestTextChannel : RestNestedChannel, IRestMessageChannel, ITextChannel
    {
        public Task TriggerTypingAsync(RestRequestOptions options = null)
            => Client.TriggerTypingAsync(Id, options);

        public Task MarkAsReadAsync(RestRequestOptions options = null)
        {
            var lastMessageId = LastMessageId;
            if (!lastMessageId.HasValue)
                throw new InvalidOperationException("Channel has no last message id.");

            return Client.MarkMessageAsReadAsync(Id, lastMessageId.Value, options);
        }

        public IDisposable Typing()
            => new TypingRepeater(Client, this);

        public Task<RestUserMessage> SendMessageAsync(string content = null, bool isTTS = false, LocalEmbed embed = null, LocalMentions mentions = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, content, isTTS, embed, mentions, options);

        public Task<RestUserMessage> SendMessageAsync(LocalAttachment attachment, string content = null, bool isTTS = false, LocalEmbed embed = null, LocalMentions mentions = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, attachment, content, isTTS, embed, mentions, options);

        public Task<RestUserMessage> SendMessageAsync(IEnumerable<LocalAttachment> attachments, string content = null, bool isTTS = false, LocalEmbed embed = null, LocalMentions mentions = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, attachments, content, isTTS, embed, mentions, options);

        public RestRequestEnumerable<RestMessage> GetMessagesEnumerable(int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetMessagesEnumerable(Id, limit, direction, startFromId, options);

        public Task<IReadOnlyList<RestMessage>> GetMessagesAsync(int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetMessagesAsync(Id, limit, direction, startFromId, options);

        public Task<RestMessage> GetMessageAsync(Snowflake messageId, RestRequestOptions options = null)
            => Client.GetMessageAsync(Id, messageId, options);

        public Task AddReactionAsync(Snowflake messageId, IEmoji emoji, RestRequestOptions options = null)
            => Client.AddReactionAsync(Id, messageId, emoji, options);

        public Task RemoveOwnReactionAsync(Snowflake messageId, IEmoji emoji, RestRequestOptions options = null)
            => Client.RemoveOwnReactionAsync(Id, messageId, emoji, options);

        public RestRequestEnumerable<RestUser> GetReactionsEnumerable(Snowflake messageId, IEmoji emoji, int limit, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetReactionsEnumerable(Id, messageId, emoji, limit, startFromId, options);

        public Task<IReadOnlyList<RestUser>> GetReactionsAsync(Snowflake messageId, IEmoji emoji, int limit = 100, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetReactionsAsync(Id, messageId, emoji, limit, startFromId, options);

        public Task<RestUserMessage> ModifyMessageAsync(Snowflake messageId, Action<ModifyMessageProperties> action, RestRequestOptions options = null)
            => Client.ModifyMessageAsync(Id, messageId, action, options);

        public Task<IReadOnlyList<RestUserMessage>> GetPinnedMessagesAsync(RestRequestOptions options = null)
            => Client.GetPinnedMessagesAsync(Id, options);

        public async Task PinMessageAsync(Snowflake messageId, RestRequestOptions options = null)
        {
            await Client.PinMessageAsync(Id, messageId, options).ConfigureAwait(false);
            LastPinTimestamp = DateTimeOffset.UtcNow;
        }

        public Task UnpinMessageAsync(Snowflake messageId, RestRequestOptions options = null)
            => Client.UnpinMessageAsync(Id, messageId, options);

        public async Task ModifyAsync(Action<ModifyTextChannelProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyChannelAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task<RestWebhook> CreateWebhookAsync(string name, Stream avatar = null, RestRequestOptions options = null)
            => Client.CreateWebhookAsync(Id, name, avatar, options);

        public Task<IReadOnlyList<RestWebhook>> GetWebhooksAsync(RestRequestOptions options = null)
            => Client.GetChannelWebhooksAsync(Id, options);

        public Task DeleteMessageAsync(Snowflake messageId, RestRequestOptions options = null)
            => Client.DeleteMessageAsync(Id, messageId, options);

        public RestRequestEnumerator<Snowflake> GetBulkMessageDeletionEnumerator(IEnumerable<Snowflake> messageIds, RestRequestOptions options = null)
            => Client.GetBulkMessageDeletionEnumerator(Id, messageIds, options);

        public Task DeleteMessagesAsync(IEnumerable<Snowflake> messageIds, RestRequestOptions options = null)
            => Client.DeleteMessagesAsync(Id, messageIds, options);

        public Task RemoveMemberReactionAsync(Snowflake messageId, Snowflake memberId, IEmoji emoji, RestRequestOptions options = null)
            => Client.RemoveMemberReactionAsync(Id, messageId, memberId, emoji, options);

        public Task ClearReactionsAsync(Snowflake messageId, IEmoji emoji = null, RestRequestOptions options = null)
            => Client.ClearReactionsAsync(Id, messageId, emoji, options);
    }
}
