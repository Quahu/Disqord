using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot
{
    public class DiscordResponseCommandResult : DiscordCommandResult
    {
        private readonly LocalMessage _message;

        public DiscordResponseCommandResult(LocalMessage message)
        {
            _message = message;
        }

        public override Task ExecuteAsync()
            => Context.Bot.SendMessageAsync(Context.ChannelId, _message);
    }
}
