using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordParameterCheckAttribute : ParameterCheckAttribute
    {
        public abstract ValueTask<CheckResult> CheckAsync(object argument, DiscordCommandContext context);

        public sealed override ValueTask<CheckResult> CheckAsync(object argument, CommandContext context)
        {
            if (context is not DiscordCommandContext discordContext)
                throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(DiscordCommandContext)}.");

            return CheckAsync(argument, discordContext);
        }
    }
}
