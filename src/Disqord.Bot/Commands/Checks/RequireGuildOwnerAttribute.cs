using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the module or command can only be executed by the guild owner.
    /// </summary>
    public class RequireGuildOwnerAttribute : DiscordGuildCheckAttribute
    {
        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
            if (context.Author.Id == context.Guild.OwnerId)
                return Success();

            return Failure("This can only be executed by the guild owner.");
        }
    }
}
