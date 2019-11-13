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
            var relationship = _currentUser._relationships.AddOrUpdate(model.Id, _ => new CachedRelationship(_client, model), (_, old) =>
            {
                old.Update(model);
                return old;
            });

            return _client._relationshipCreated.InvokeAsync(new RelationshipCreatedEventArgs(relationship));
        }
    }
}
