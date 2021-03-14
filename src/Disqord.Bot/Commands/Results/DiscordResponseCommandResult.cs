using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot
{
    public class DiscordResponseCommandResult : DiscordCommandResult
    {
        public virtual LocalMessage Message { get; }

        public DiscordResponseCommandResult(DiscordCommandContext context, LocalMessage message)
            : base(context)
        {
            Message = message;
        }

        public override Task ExecuteAsync()
            => Context.Bot.SendMessageAsync(Context.ChannelId, Message);
    }
}
