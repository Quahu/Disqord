using System.Threading.Tasks;
using Disqord.Gateway.Api;

namespace Disqord.Gateway.Default
{
    public partial class DefaultGatewayDispatcher
    {
        private Task GuildUpdateAsync(GatewayDispatchReceivedEventArgs e)
        {
            //return _guildUpdatedEvent.InvokeAsync(this, new GuildUpdatedEventArgs());
            return Task.CompletedTask;
        }
    }
}
