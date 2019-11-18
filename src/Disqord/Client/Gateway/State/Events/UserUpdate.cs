using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleUserUpdateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<UserModel>(payload.D);
            var currentUserBefore = _currentUser.Clone();
            _currentUser.Update(model);

            return _client._userUpdated.InvokeAsync(new UserUpdatedEventArgs(currentUserBefore, _currentUser));
        }
    }
}
