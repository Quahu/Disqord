using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientState
    {
        public Task HandleWebhooksUpdateAsync(PayloadModel payload)
        {
            var model = payload.D.ToType<WebhooksUpdateModel>();
            return _client._webhooksUpdated.InvokeAsync(new WebhooksUpdatedEventArgs(_client, model.GuildId, model.ChannelId));
        }
    }
}
