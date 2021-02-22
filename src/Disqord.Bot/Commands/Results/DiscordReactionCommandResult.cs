using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot
{
    public class DiscordReactionCommandResult : DiscordCommandResult
    {
        private IEmoji _emoji;

        public DiscordReactionCommandResult(IEmoji emoji)
        {
            _emoji = emoji;
        }

        public override Task ExecuteAsync()
            => Context.Bot.CreateReactionAsync(Context.ChannelId, Context.Message.Id, _emoji);
    }
}