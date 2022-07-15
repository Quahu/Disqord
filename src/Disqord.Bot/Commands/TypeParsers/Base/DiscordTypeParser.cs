using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Qmmands;
using Qmmands.Default;

namespace Disqord.Bot.Commands;

/// <summary>
///     Represents type parsing for the given <typeparamref name="T"/> type.
/// </summary>
/// <remarks>
///     This type restricts the input context to a <see cref="IDiscordCommandContext"/>.
/// </remarks>
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
    public abstract ValueTask<ITypeParserResult<T>> ParseAsync(IDiscordCommandContext context, IParameter parameter, ReadOnlyMemory<char> value);

    /// <inheritdoc/>
    public sealed override ValueTask<ITypeParserResult<T>> ParseAsync(ICommandContext context, IParameter parameter, ReadOnlyMemory<char> value)
    {
        if (context is not IDiscordCommandContext discordContext)
            throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(IDiscordCommandContext)}.");

        return ParseAsync(discordContext, parameter, value);
    }
}
