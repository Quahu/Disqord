using System.Threading.Tasks;
using Disqord.Gateway.Api;

namespace Disqord.Gateway.Default
{
    public partial class DefaultGatewayDispatcher
    {
        private Task MessageDeleteBulkAsync(GatewayDispatchReceivedEventArgs e)
        {
            //return _messageUpdatedEvent.InvokeAsync(this, new MessageUpdatedEventArgs());
            return Task.CompletedTask;
        }
    }
}
