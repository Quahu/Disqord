using System.Threading.Tasks;
using Disqord.Utilities.Binding;

namespace Disqord.Bot
{
    public delegate Task CommandQueueDelegate(string input, DiscordCommandContext context);

    public interface ICommandQueue : IBindable<DiscordBotBase>
    {
        DiscordBotBase Bot { get; }

        void Post(string input, DiscordCommandContext context, CommandQueueDelegate func);
    }
}
