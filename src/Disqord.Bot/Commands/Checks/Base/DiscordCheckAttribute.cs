using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordCheckAttribute : CheckAttribute
    {
        public abstract ValueTask<CheckResult> CheckAsync(DiscordCommandContext context);

        public sealed override ValueTask<CheckResult> CheckAsync(CommandContext context)
        {
            if (context is not DiscordCommandContext discordContext)
                throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(DiscordCommandContext)}.");

            return CheckAsync(discordContext);
        }
    }
}
