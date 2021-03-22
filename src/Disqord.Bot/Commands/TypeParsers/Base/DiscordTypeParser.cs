using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordTypeParser<T> : TypeParser<T>
    {
        public abstract ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, DiscordCommandContext context); 
        
        public override sealed ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            if (context is not DiscordCommandContext discordContext)
                throw new InvalidOperationException($"The {GetType().Name} only accepts a DiscordCommandContext.");

            return ParseAsync(parameter, value, discordContext);
        }
    }
}
