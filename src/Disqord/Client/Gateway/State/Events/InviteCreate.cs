using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleInviteCreateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<InviteCreateModel>(payload.D);
            CachedGuild guild = null;
            CachedChannel channel;
            CachedUser inviter;
            if (model.GuildId != null)
            {
                guild = GetGuild(model.GuildId.Value);
                channel = guild.GetChannel(model.ChannelId);
                inviter = guild.GetMember(model.Inviter.Id);
            }
            else
            {
                channel = GetPrivateChannel(model.ChannelId);
                inviter = GetUser(model.Inviter.Id);
            }

            if (inviter == null)
                inviter = new CachedUnknownUser(_client, model.Inviter);

            return _client._inviteCreated.InvokeAsync(new InviteCreatedEventArgs(
                _client, guild, new OptionalSnowflakeEntity<CachedChannel>(channel, model.ChannelId),
                inviter, model.Code, model.Temporary, model.MaxUses, model.MaxAge, model.CreatedAt));
        }
    }
}
