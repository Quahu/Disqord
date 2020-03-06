using System;
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
