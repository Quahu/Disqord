using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

public abstract class DiscordGuildCheckAttribute : DiscordCheckAttribute
{
    public abstract ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context);

    public sealed override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context.GuildId == null)
            return Results.Failure("This can only be executed within a guild.");

        if (context is not IDiscordGuildCommandContext discordContext)
            throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(IDiscordGuildCommandContext)}.");

        return CheckAsync(discordContext);
    }
}
