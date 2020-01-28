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

        public RestRequestEnumerable<RestUser> GetReactionsEnumerable(IEmoji emoji, int limit, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetReactionsEnumerable(ChannelId, Id, emoji, limit, startFromId, options);

        public Task<IReadOnlyList<RestUser>> GetReactionsAsync(IEmoji emoji, int limit = 100, Snowflake? startFromId = null, RestRequestOptions options = null)
            => Client.GetReactionsAsync(ChannelId, Id, emoji, limit, startFromId, options);

        public Task ClearReactionsAsync(IEmoji emoji = null, RestRequestOptions options = null)
            => Client.ClearReactionsAsync(ChannelId, Id, emoji, options);

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteMessageAsync(ChannelId, Id, options);
    }
}
