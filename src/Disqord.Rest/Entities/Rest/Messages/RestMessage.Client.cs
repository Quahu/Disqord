using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public abstract partial class RestMessage : RestSnowflakeEntity, IMessage
    {
        public Task AddReactionAsync(IEmoji emoji, RestRequestOptions options = null)
            => Client.AddReactionAsync(ChannelId, Id, emoji, options);

        public Task RemoveOwnReactionAsync(IEmoji emoji, RestRequestOptions options = null)
            => Client.RemoveOwnReactionAsync(ChannelId, Id, emoji, options);

        public Task RemoveMemberReactionAsync(Snowflake memberId, IEmoji emoji, RestRequestOptions options = null)
            => Client.RemoveMemberReactionAsync(ChannelId, Id, memberId, emoji, options);

        public RestRequestEnumerable<RestUser> GetReactionsEnumerable(IEmoji emoji, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null)
            => Client.GetReactionsEnumerable(ChannelId, Id, emoji, limit, direction, startFromId);

        public Task<IReadOnlyList<RestUser>> GetReactionsAsync(IEmoji emoji, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetReactionsAsync(ChannelId, Id, emoji, limit, direction, startFromId, options);

        public Task ClearReactionsAsync(RestRequestOptions options = null)
            => Client.ClearReactionsAsync(ChannelId, Id, options);

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteMessageAsync(ChannelId, Id, options);
    }
}
