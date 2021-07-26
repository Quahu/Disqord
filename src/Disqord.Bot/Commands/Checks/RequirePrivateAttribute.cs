using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the module or command can only be executed in private (e.g. direct) channels.
    /// </summary>
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
