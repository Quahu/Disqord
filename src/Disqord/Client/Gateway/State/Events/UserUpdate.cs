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
            var user = GetUser(model.Id);
            var userBefore = user.Clone();
            user.Update(model);

            return _client._userUpdated.InvokeAsync(new UserUpdatedEventArgs(userBefore, user));
        }
    }
}
