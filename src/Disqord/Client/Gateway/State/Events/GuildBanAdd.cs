using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleGuildBanAddAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<GuildBanAddModel>(payload.D);
            var guild = GetGuild(model.GuildId);
            var user = guild._members.TryGetValue(model.User.Id, out var member)
                ? member
                : GetSharedOrUnknownUser(model.User);

            return _client._memberBanned.InvokeAsync(new MemberBannedEventArgs(guild, user));
        }
    }
}
