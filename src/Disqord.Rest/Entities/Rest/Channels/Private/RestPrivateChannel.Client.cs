﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public abstract partial class RestPrivateChannel : RestChannel, IRestMessageChannel, IPrivateChannel
    {
        public Task TriggerTypingAsync(RestRequestOptions options = null)
            => Client.TriggerTypingAsync(Id, options);

        public Task MarkAsReadAsync(RestRequestOptions options = null)
            => RestImplementation.MarkAsReadAsync(this, options);

        public IDisposable Typing()
            => new TypingRepeater(Client, this);

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

        public Task DeleteMessageAsync(Snowflake messageId, RestRequestOptions options = null)
            => Client.DeleteMessageAsync(Id, messageId, options);

        public Task<IReadOnlyList<RestUserMessage>> GetPinnedMessagesAsync(RestRequestOptions options = null)
            => Client.GetPinnedMessagesAsync(Id, options);

        public async Task PinMessageAsync(Snowflake messageId, RestRequestOptions options = null)
        {
            await Client.PinMessageAsync(Id, messageId, options).ConfigureAwait(false);
            LastPinTimestamp = DateTimeOffset.UtcNow;
        }

        public Task UnpinMessageAsync(Snowflake messageId, RestRequestOptions options = null)
            => Client.UnpinMessageAsync(Id, messageId, options);
    }
}
