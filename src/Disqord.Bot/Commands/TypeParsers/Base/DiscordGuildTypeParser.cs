using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Represents type parsing for the given <typeparamref name="T"/> type.
    ///     This type restricts the input context to a <see cref="DiscordGuildCommandContext"/>.
    /// </summary>
    /// <typeparam name="T"> The parsed type. </typeparam>
    public abstract class DiscordGuildTypeParser<T> : DiscordTypeParser<T>
    {
        /// <inheritdoc cref="ParseAsync(Parameter, string, DiscordCommandContext)"/>
        public abstract ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, DiscordGuildCommandContext context);

        /// <inheritdoc/>
        public sealed override ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, DiscordCommandContext context)
        {
            if (context.GuildId == null)
                return Failure("This can only be executed within a guild.");

            if (context is not DiscordGuildCommandContext discordContext)
                throw new InvalidOperationException($"The {GetType().Name} only accepts a DiscordGuildCommandContext.");

            return ParseAsync(parameter, value, discordContext);
        }
    }
}
