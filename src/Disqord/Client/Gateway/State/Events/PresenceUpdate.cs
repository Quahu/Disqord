using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandlePresenceUpdateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<PresenceUpdateModel>(payload.D);
            if (model.GuildId == null)
            {
                if (!_users.TryGetValue(model.User.Id, out var user))
                {
                    if (!model.User.Username.HasValue)
                        return Task.CompletedTask;

                    user = GetOrAddSharedUser(model.User);
                }

                user.Update(model);
            }
            else
            {
                var guild = GetGuild(model.GuildId.Value);
                var member = guild.GetMember(model.User.Id);
                if (member == null)
                {
                    if (!model.User.Username.HasValue)
                        return Task.CompletedTask;

                    member = CreateMember(guild, new MemberModel
                    {
                        Nick = model.Nick,
                        Roles = model.Roles
                    }, model.User);
                }

                member.Update(model);
            }

            return Task.CompletedTask;
        }
    }
}
