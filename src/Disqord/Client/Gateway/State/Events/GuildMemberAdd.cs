using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildMemberAddAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<GuildMemberAddModel>(payload.D);
            var guild = GetGuild(model.GuildId);
            var member = AddMember(guild, model, model.User);

            return _client._memberJoined.InvokeAsync(new MemberJoinedEventArgs(member));
        }
    }
}
