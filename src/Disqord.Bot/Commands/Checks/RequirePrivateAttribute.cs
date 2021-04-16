using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public class RequirePrivateAttribute : DiscordCheckAttribute
    {
        public RequirePrivateAttribute()
        { }

        public override sealed ValueTask<CheckResult> CheckAsync(DiscordCommandContext context)
        {
            if (context.GuildId == null)
                return Success();

            return Failure("This can only be executed outside of a guild.");
        }
    }
}
