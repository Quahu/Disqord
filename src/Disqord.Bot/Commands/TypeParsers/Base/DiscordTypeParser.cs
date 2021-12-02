using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    /// <summary>
    ///     Represents type parsing for the given <typeparamref name="T"/> type.
    ///     This type restricts the input context to a <see cref="DiscordCommandContext"/>.
    /// </summary>
    /// <typeparam name="T"> The parsed type. </typeparam>
    public abstract class DiscordTypeParser<T> : TypeParser<T>
    {
        /// <summary>
        ///     Attempts to parse the provided raw argument to the <typeparamref name="T"/> type.
        /// </summary>
        /// <param name="parameter"> The currently parsed <see cref="Parameter"/>. </param>
        /// <param name="value"> The raw argument to parse. </param>
        /// <param name="context"> The context used for execution. </param>
        /// <returns>
        ///     A <see cref="ValueTask{TResult}"/> representing the parsing work
        ///     that wraps the parsing result.
        /// </returns>
        public abstract ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, DiscordCommandContext context);

        /// <inheritdoc/>
        public sealed override ValueTask<TypeParserResult<T>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            if (context is not DiscordCommandContext discordContext)
                throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(DiscordCommandContext)}.");

            return ParseAsync(parameter, value, discordContext);
        }
    }
}
