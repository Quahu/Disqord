using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <inheritdoc/>
/// <summary>
///     The type additionally typechecks the <see cref="ICommandContext"/>
///     to be an <see cref="IDiscordCommandContext"/>.
/// </summary>
public abstract class DiscordParameterCheckAttribute : ParameterCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="DiscordParameterCheckAttribute"/>.
    /// </summary>
    protected DiscordParameterCheckAttribute()
    { }

    /// <inheritdoc cref="ParameterCheckAttribute.CheckAsync"/>
    public abstract ValueTask<IResult> CheckAsync(IDiscordCommandContext context, IParameter parameter, object? argument);

    /// <inheritdoc/>
    public sealed override ValueTask<IResult> CheckAsync(ICommandContext context, IParameter parameter, object? argument)
    {
        if (context is not IDiscordCommandContext discordContext)
            throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(IDiscordCommandContext)}.");

        return CheckAsync(discordContext, parameter, argument);
    }
}
