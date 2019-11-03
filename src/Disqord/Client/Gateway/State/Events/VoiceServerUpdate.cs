using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleVoiceServerUpdateAsync(PayloadModel payload)
        {
            var model = Serializer.ToObject<VoiceServerUpdateModel>(payload.D);
            if (model.GuildId == null)
                return Task.CompletedTask;

            var guild = GetGuild(model.GuildId.Value);
            return _client._voiceServerUpdated.InvokeAsync(new VoiceServerUpdatedEventArgs(guild, model.Token, model.Endpoint));
        }
    }
}
