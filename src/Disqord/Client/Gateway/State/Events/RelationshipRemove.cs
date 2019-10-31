using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleRelationshipRemoveAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<RelationshipModel>(payload.D);
            _currentUser.TryRemoveRelationship(model.Id, out var relationship);

            return _client._relationshipDeleted.InvokeAsync(new RelationshipDeletedEventArgs(relationship));
        }
    }
}
