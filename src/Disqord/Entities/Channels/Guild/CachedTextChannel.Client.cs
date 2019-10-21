﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public sealed partial class CachedTextChannel : CachedNestedChannel, ITextChannel, ICachedMessageChannel
    {
        public Task TriggerTypingAsync(RestRequestOptions options = null)
            => Client.TriggerTypingAsync(Id, options);

        public Task MarkAsReadAsync(RestRequestOptions options = null)
            => RestImplementation.MarkAsReadAsync(this, options);

        public IDisposable Typing()
            => new TypingRepeater(Client.RestClient, this);

        public Task<RestUserMessage> SendMessageAsync(string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, content, isTTS, embed, options);

        public Task<RestUserMessage> SendMessageAsync(LocalAttachment attachment, string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, attachment, content, isTTS, embed, options);

        public Task<RestUserMessage> SendMessageAsync(IEnumerable<LocalAttachment> attachments, string content = null, bool isTTS = false, Embed embed = null, RestRequestOptions options = null)
            => Client.SendMessageAsync(Id, attachments, content, isTTS, embed, options);

        public RestRequestEnumerator<RestMessage> GetMessagesEnumerator(int limit, RetrievalDirection? direction = null, Snowflake? startFromId = null)
            => Client.GetMessagesEnumerator(Id, limit, direction, startFromId);

        public Task<IReadOnlyList<RestMessage>> GetMessagesAsync(int limit = 100, RetrievalDirection? direction = null, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetMessagesAsync(Id, limit, direction, startFromId, options);

        public Task<RestMessage> GetMessageAsync(Snowflake messageId, RestRequestOptions options = null)
            => Client.GetMessageAsync(Id, messageId, options);

        public Task AddReactionAsync(Snowflake messageId, IEmoji emoji, RestRequestOptions options = null)
            => Client.AddReactionAsync(Id, messageId, emoji, options);

        public Task RemoveOwnReactionAsync(Snowflake messageId, IEmoji emoji, RestRequestOptions options = null)
            => Client.RemoveOwnReactionAsync(Id, messageId, emoji, options);

        public RestRequestEnumerator<RestUser> GetReactionEnumerator(Snowflake messageId, IEmoji emoji, int limit, RetrievalDirection? direction = null, Snowflake? startFromId = null)
            => Client.GetReactionEnumerator(Id, messageId, emoji, limit, direction, startFromId);

        public Task<IReadOnlyList<RestUser>> GetReactionsAsync(Snowflake messageId, IEmoji emoji, int limit = 100, RetrievalDirection? direction = null, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetReactionsAsync(Id, messageId, emoji, limit, direction, startFromId, options);

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
            var model = await Client.RestClient.InternalModifyChannelAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task<RestWebhook> CreateWebhookAsync(string name, LocalAttachment avatar = null, RestRequestOptions options = null)
            => Client.CreateWebhookAsync(Id, name, avatar, options);

        public Task<IReadOnlyList<RestWebhook>> GetWebhooksAsync(RestRequestOptions options = null)
            => Client.GetChannelWebhooksAsync(Id, options);

        public Task DeleteMessageAsync(Snowflake messageId, RestRequestOptions options = null)
            => Client.DeleteMessageAsync(Id, messageId, options);

        public RestRequestEnumerator<Snowflake> GetBulkMessageDeletionEnumerator(IEnumerable<Snowflake> messageIds)
            => Client.GetBulkMessageDeletionEnumerator(Id, messageIds);

        public Task DeleteMessagesAsync(IEnumerable<Snowflake> messageIds, RestRequestOptions options = null)
            => Client.DeleteMessagesAsync(Id, messageIds, options);

        public Task RemoveMemberReactionAsync(Snowflake messageId, Snowflake memberId, IEmoji emoji, RestRequestOptions options = null)
            => Client.RemoveMemberReactionAsync(Id, messageId, memberId, emoji, options);

        public Task RemoveAllReactionsAsync(Snowflake messageId, RestRequestOptions options = null)
            => Client.RemoveAllReactionsAsync(Id, messageId, options);
    }
}
