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

        public override Task ExecuteAsync(DiscordCommandContext context)
            => context.Bot.CreateReactionAsync(context.Message.ChannelId, context.Message.Id, _emoji);
    }
}