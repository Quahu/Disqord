using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <inheritdoc cref="CheckAttribute"/>
/// <summary>
///     The type additionally typechecks the <see cref="ICommandContext"/>
///     to be an <see cref="IDiscordGuildCommandContext"/>.
/// </summary>
public abstract class DiscordGuildCheckAttribute : DiscordCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="DiscordGuildCheckAttribute"/>.
    /// </summary>
    protected DiscordGuildCheckAttribute()
    { }

    /// <inheritdoc cref="CheckAttribute.CheckAsync"/>
    public abstract ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context);

    /// <inheritdoc/>
    public sealed override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context.GuildId == null)
            return Results.Failure("This can only be executed within a guild.");

        if (context is not IDiscordGuildCommandContext discordContext)
            throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(IDiscordGuildCommandContext)}.");

        return CheckAsync(discordContext);
    }
}
