using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the module or command can only be executed by the bot owners.
    /// </summary>
    public class RequireBotOwnerAttribute : DiscordCheckAttribute
    {
        public override async ValueTask<CheckResult> CheckAsync(DiscordCommandContext context)
        {
            if (await context.Bot.IsOwnerAsync(context.Author).ConfigureAwait(false))
                return Success();

            return Failure("This can only be executed by the bot owners.");
        }
    }
}
