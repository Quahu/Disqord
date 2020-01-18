using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleInviteDeleteAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<InviteDeleteModel>(payload.D);
            return _client._inviteDeleted.InvokeAsync(new InviteDeletedEventArgs(_client, model.GuildId, model.ChannelId, model.Code));
        }
    }
}
