using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordGuildCheckAttribute : DiscordCheckAttribute
    {
        public abstract ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context);

        public override sealed ValueTask<CheckResult> CheckAsync(DiscordCommandContext context)
        {
            if (context.GuildId == null)
                return Failure("This can only be executed within a guild.");

            if (context is not DiscordGuildCommandContext discordContext)
                throw new InvalidOperationException($"The {GetType().Name} only accepts a DiscordGuildCommandContext.");

            return CheckAsync(discordContext);
        }
    }
}
