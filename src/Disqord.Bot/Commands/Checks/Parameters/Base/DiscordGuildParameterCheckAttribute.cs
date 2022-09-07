using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <inheritdoc cref="ParameterCheckAttribute"/>
/// <summary>
///     The type additionally typechecks the <see cref="ICommandContext"/>
///     to be an <see cref="IDiscordGuildCommandContext"/>.
/// </summary>
public abstract class DiscordGuildParameterCheckAttribute : DiscordParameterCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="DiscordGuildParameterCheckAttribute"/>.
    /// </summary>
    protected DiscordGuildParameterCheckAttribute()
    { }

    /// <inheritdoc cref="ParameterCheckAttribute.CheckAsync"/>
    public abstract ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context, IParameter parameter, object? argument);

    /// <inheritdoc/>
    public sealed override ValueTask<IResult> CheckAsync(IDiscordCommandContext context, IParameter parameter, object? argument)
    {
        if (context.GuildId == null)
            return Results.Failure("This can only be executed within a guild.");

        if (context is not IDiscordGuildCommandContext discordContext)
            throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(IDiscordGuildCommandContext)}.");

        return CheckAsync(discordContext, parameter, argument);
    }
}
