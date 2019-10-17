using Disqord.Rest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IMessageChannel : IChannel, IMessagable
    {
        Task TriggerTypingAsync(RestRequestOptions options = null);

        Task MarkAsReadAsync(RestRequestOptions options = null);

        IDisposable Typing();

        RestRequestEnumerator<RestMessage> GetMessagesEnumerator(int limit, RetrievalDirection? direction = null, Snowflake? startFromId = null);

        Task<IReadOnlyList<RestMessage>> GetMessagesAsync(int limit = 100, RetrievalDirection? direction = null, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task<RestMessage> GetMessageAsync(Snowflake messageId, RestRequestOptions options = null);

        Task AddReactionAsync(Snowflake messageId, IEmoji emoji, RestRequestOptions options = null);
        
        Task RemoveOwnReactionAsync(Snowflake messageId, IEmoji emoji, RestRequestOptions options = null);
        
        RestRequestEnumerator<RestUser> GetReactionEnumerator(Snowflake messageId, IEmoji emoji, int limit, RetrievalDirection? direction = null, Snowflake? startFromId = null);
        
        Task<IReadOnlyList<RestUser>> GetReactionsAsync(Snowflake messageId, IEmoji emoji, int limit = 100, RetrievalDirection? direction = null, Snowflake? startFromId = null, RestRequestOptions options = null);
        
        Task<RestMessage> ModifyMessageAsync(Snowflake messageId, Action<ModifyMessageProperties> action, RestRequestOptions options = null);

        Task DeleteMessageAsync(Snowflake messageId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestUserMessage>> GetPinnedMessagesAsync(RestRequestOptions options = null);

        Task PinMessageAsync(Snowflake messageId, RestRequestOptions options = null);

        Task UnpinMessageAsync(Snowflake messageId, RestRequestOptions options = null);
    }
}
