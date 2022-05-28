using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

public abstract class DiscordGuildParameterCheckAttribute : DiscordParameterCheckAttribute
{
    protected DiscordGuildParameterCheckAttribute()
    { }

    public abstract ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context, IParameter parameter, object? argument);

    public sealed override ValueTask<IResult> CheckAsync(IDiscordCommandContext context, IParameter parameter, object? argument)
    {
        if (context.GuildId == null)
            return Results.Failure("This can only be executed within a guild.");

        if (context is not IDiscordGuildCommandContext discordContext)
            throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(IDiscordGuildCommandContext)}.");

        return CheckAsync(discordContext, parameter, argument);
    }
}
