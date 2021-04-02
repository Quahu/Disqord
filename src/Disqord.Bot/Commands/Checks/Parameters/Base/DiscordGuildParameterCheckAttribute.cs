using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordGuildParameterCheckAttribute : DiscordParameterCheckAttribute
    {
        public abstract ValueTask<CheckResult> CheckAsync(object argument, DiscordGuildCommandContext context);

        public override sealed ValueTask<CheckResult> CheckAsync(object argument, DiscordCommandContext context)
        {
            if (context.GuildId == null)
                return Failure("This can only be executed within a guild.");

            if (context is not DiscordGuildCommandContext discordContext)
                throw new InvalidOperationException($"The {GetType().Name} only accepts a DiscordGuildCommandContext.");

            return CheckAsync(argument, discordContext);
        }
    }
}
