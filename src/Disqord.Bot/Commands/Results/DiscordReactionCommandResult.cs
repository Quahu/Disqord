using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot
{
    public class DiscordReactionCommandResult : DiscordCommandResult
    {
        public virtual LocalEmoji Emoji { get; }

        public DiscordReactionCommandResult(DiscordCommandContext context, LocalEmoji emoji)
            : base(context)
        {
            Emoji = emoji;
        }

        public override Task ExecuteAsync()
            => Context.Message.AddReactionAsync(Emoji);
    }
}
