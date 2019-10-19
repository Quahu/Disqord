using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public Task CreateRelationshipAsync(Snowflake userId, RelationshipType? type = null, RestRequestOptions options = null)
        {
            if (type != null && !Enum.IsDefined(typeof(RelationshipType), type.Value))
                throw new ArgumentOutOfRangeException(nameof(type));

            return ApiClient.CreateRelationshipAsync(userId, type, options);
        }

        public Task DeleteRelationshipAsync(Snowflake userId, RestRequestOptions options = null)
            => ApiClient.DeleteRelationshipAsync(userId, options);

        public Task SendFriendRequestAsync(string name, string discriminator, RestRequestOptions options = null)
            => ApiClient.SendFriendRequestAsync(name, discriminator, options);

        public async Task<IReadOnlyList<RestRelationship>> GetRelationshipsAsync(RestRequestOptions options = null)
        {
            var models = await ApiClient.GetRelationshipsAsync(options).ConfigureAwait(false);
            return models.Select(x => new RestRelationship(this, x)).ToImmutableArray();
        }

        public async Task<IReadOnlyList<RestUser>> GetMutualFriendsAsync(Snowflake userId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetMutualFriendsAsync(userId, options).ConfigureAwait(false);
            return models.Select(x => new RestUser(this, x)).ToImmutableArray();
        }
    }
}
