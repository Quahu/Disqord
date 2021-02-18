using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageCreateHandler : Handler<MessageJsonModel, MessageReceivedEventArgs>
    {
        public override async Task<MessageReceivedEventArgs> HandleDispatchAsync(MessageJsonModel model)
        {
            var message = TransientMessage.Create(Client, model);
            return new MessageReceivedEventArgs(message);
        }
    }
}
