using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public Task SendOrAcceptFriendRequestAsync(Snowflake userId, RestRequestOptions options = null)
            => ApiClient.CreateRelationshipAsync(userId, null, options);

        public Task SendOrAcceptFriendRequestAsync(string name, string discriminator, RestRequestOptions options = null)
            => ApiClient.SendFriendRequestAsync(name, discriminator, options);

        public Task BlockUserAsync(Snowflake userId, RestRequestOptions options = null)
            => ApiClient.CreateRelationshipAsync(userId, RelationshipType.Blocked, options);

        public Task DeleteRelationshipAsync(Snowflake userId, RestRequestOptions options = null)
            => ApiClient.DeleteRelationshipAsync(userId, options);

        public async Task<IReadOnlyList<RestRelationship>> GetRelationshipsAsync(RestRequestOptions options = null)
        {
            var models = await ApiClient.GetRelationshipsAsync(options).ConfigureAwait(false);
            return models.ToReadOnlyList(this, (x, @this) => new RestRelationship(@this, x));
        }

        public async Task<IReadOnlyList<RestUser>> GetMutualFriendsAsync(Snowflake userId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetMutualFriendsAsync(userId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(this, (x, @this) => new RestUser(@this, x));
        }
    }
}
