using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <inheritdoc/>
/// <summary>
///     The type additionally typechecks the <see cref="ICommandContext"/>
///     to be an <see cref="IDiscordCommandContext"/>.
/// </summary>
public abstract class DiscordCheckAttribute : CheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="DiscordCheckAttribute"/>.
    /// </summary>
    protected DiscordCheckAttribute()
    { }

    /// <inheritdoc cref="CheckAttribute.CheckAsync"/>
    public abstract ValueTask<IResult> CheckAsync(IDiscordCommandContext context);

    /// <inheritdoc/>
    public sealed override ValueTask<IResult> CheckAsync(ICommandContext context)
    {
        if (context is not IDiscordCommandContext discordContext)
            throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(IDiscordCommandContext)}.");

        return CheckAsync(discordContext);
    }
}
