using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Represents type parsing for the given <typeparamref name="T"/> type.
/// </summary>
/// <remarks>
///     This type restricts the input context to a <see cref="IDiscordGuildCommandContext"/>.
/// </remarks>
/// <typeparam name="T"> The parsed type. </typeparam>
public abstract class DiscordGuildTypeParser<T> : DiscordTypeParser<T>
{
    /// <inheritdoc cref="ParseAsync(IDiscordCommandContext, IParameter, ReadOnlyMemory{char})"/>
    public abstract ValueTask<ITypeParserResult<T>> ParseAsync(IDiscordGuildCommandContext context, IParameter parameter, ReadOnlyMemory<char> value);

    /// <inheritdoc/>
    public sealed override ValueTask<ITypeParserResult<T>> ParseAsync(IDiscordCommandContext context, IParameter parameter, ReadOnlyMemory<char> value)
    {
        if (context.GuildId == null)
            return Failure("This can only be executed within a guild.");

        if (context is not IDiscordGuildCommandContext discordContext)
            throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(IDiscordGuildCommandContext)}.");

        return ParseAsync(discordContext, parameter, value);
    }
}
