using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleRelationshipAddAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<RelationshipModel>(payload.D);
            if (_currentUser.Relationships.TryGetValue(model.Id, out var relationship))
            {
                relationship.Update(model);
            }
            else
            {
                relationship = new CachedRelationship(_client, model);
                _currentUser.TryAddRelationship(relationship);
            }

            return _client._relationshipCreated.InvokeAsync(new RelationshipCreatedEventArgs(relationship));
        }
    }
}
