using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task CreateRelationshipAsync(Snowflake userId, RelationshipType? type = null, RestRequestOptions options = null);

        Task DeleteRelationshipAsync(Snowflake userId, RestRequestOptions options = null);

        Task SendFriendRequestAsync(string name, string discriminator, RestRequestOptions options = null);

        Task<IReadOnlyList<RestRelationship>> GetRelationshipsAsync(RestRequestOptions options = null);

        Task<IReadOnlyList<RestUser>> GetMutualFriendsAsync(Snowflake userId, RestRequestOptions options = null);
    }
}
