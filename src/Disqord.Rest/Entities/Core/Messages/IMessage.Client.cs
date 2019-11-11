using Disqord.Rest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface IMessage : ISnowflakeEntity, IDeletable
    {
        Task AddReactionAsync(IEmoji emoji, RestRequestOptions options = null);

        Task RemoveOwnReactionAsync(IEmoji emoji, RestRequestOptions options = null);

        Task RemoveMemberReactionAsync(Snowflake memberId, IEmoji emoji, RestRequestOptions options = null);

        RestRequestEnumerator<RestUser> GetReactionEnumerator(IEmoji emoji, int limit, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null);

        Task<IReadOnlyList<RestUser>> GetReactionsAsync(IEmoji emoji, int limit = 100, RetrievalDirection direction = RetrievalDirection.Before, Snowflake? startFromId = null, RestRequestOptions options = null);

        Task RemoveAllReactionsAsync(RestRequestOptions options = null);
    }
}
