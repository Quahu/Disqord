using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task SendOrAcceptFriendRequestAsync(Snowflake userId, RestRequestOptions options = null);

        Task SendOrAcceptFriendRequestAsync(string name, string discriminator, RestRequestOptions options = null);

        Task BlockUserAsync(Snowflake userId, RestRequestOptions options = null);

        Task DeleteRelationshipAsync(Snowflake userId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestRelationship>> GetRelationshipsAsync(RestRequestOptions options = null);

        Task<IReadOnlyList<RestUser>> GetMutualFriendsAsync(Snowflake userId, RestRequestOptions options = null);
    }
}
