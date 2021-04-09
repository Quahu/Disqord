using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordCheckAttribute : CheckAttribute
    {
        public abstract ValueTask<CheckResult> CheckAsync(DiscordCommandContext context);

        public override sealed ValueTask<CheckResult> CheckAsync(CommandContext context)
        {
            if (context is not DiscordCommandContext discordContext)
                throw new InvalidOperationException($"The {GetType().Name} only accepts a DiscordCommandContext.");

            return CheckAsync(discordContext);
        }
    }
}
