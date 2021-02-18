using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireChannelAttribute : CheckAttribute
    {
        public Snowflake Id { get; }

        public RequireChannelAttribute(ulong id)
        {
            Id = id;
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var context = _ as DiscordCommandContext;

            if (context.Message.ChannelId == Id)
                return Success();

            return Failure($"This can only be executed in the channe with ID {Id}.");
        }
    }
}
