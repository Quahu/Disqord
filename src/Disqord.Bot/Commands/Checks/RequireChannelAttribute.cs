using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireChannelAttribute : DiscordCheckAttribute
    {
        public Snowflake Id { get; }

        public RequireChannelAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(DiscordCommandContext context)
        {
            if (context.ChannelId == Id)
                return Success();

            return Failure($"This can only be executed in the channel with the ID {Id}.");
        }
    }
}
