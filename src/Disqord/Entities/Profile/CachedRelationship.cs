using System;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a relationship with a user.
    /// </summary>
    public sealed class CachedRelationship : CachedSnowflakeEntity, IRelationship
    {
        /// <summary>
        ///     Gets the user the relationship is for.
        /// </summary>
        public CachedUser User { get; }

        /// <summary>
        ///     Gets the type of the relationship.
        /// </summary>
        public RelationshipType Type { get; private set; }

        IUser IRelationship.User => User;

        internal CachedRelationship(DiscordClient client, RelationshipModel model) : base(client, model.Id)
        {
            User = client._users.GetOrAdd(model.Id, _ =>
            {
                var user = new CachedSharedUser(client, model.User);
                user.References++;
                return user;
            });
            Update(model);
        }

        internal void Update(RelationshipModel model)
        {
            Type = model.Type;
        }

        /// <summary>
        ///     Accepts this relationship if <see cref="Type"/> is <see cref="RelationshipType.IncomingFriendRequest"/>. Throws otherwise.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Relationship's type must be an incoming friend request.
        /// </exception>
        public Task AcceptAsync(RestRequestOptions options = null)
        {
            if (Type != RelationshipType.IncomingFriendRequest)
                throw new InvalidOperationException("Relationship's type must be an incoming friend request.");

            return Client.CreateRelationshipAsync(User.Id, options: options);
        }

        /// <summary>
        ///     Deletes this relationship regardless of <see cref="Type"/>.
        /// </summary>
        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteRelationshipAsync(User.Id, options);

        public override string ToString()
            => $"{User}: {Type}";
    }
}
