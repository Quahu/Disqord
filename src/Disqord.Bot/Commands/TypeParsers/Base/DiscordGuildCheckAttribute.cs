using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordGuildTypeParser<T> : DiscordTypeParser<T>
    {
        public abstract ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, DiscordGuildCommandContext context); 

        public override sealed ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, DiscordCommandContext context)
        {
            if (context.GuildId == null)
                return Failure("This can only be executed within a guild.");

            if (context is not DiscordGuildCommandContext discordContext)
                throw new InvalidOperationException($"The {GetType().Name} only accepts a DiscordGuildCommandContext.");

            return ParseAsync(parameter, value, discordContext);
        }
    }
}
