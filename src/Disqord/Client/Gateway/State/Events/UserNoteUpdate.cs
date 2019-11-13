using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleUserNoteUpdateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<UserNoteUpdateModel>(payload.D);
            string oldNote = null;
            _currentUser._notes.AddOrUpdate(model.Id, model.Note, (_, old) =>
            {
                oldNote = old;
                return model.Note;
            });

            return _client._userNoteUpdated.InvokeAsync(new UserNoteUpdatedEventArgs(_client, model.Id, oldNote, model.Note));
        }
    }
}
