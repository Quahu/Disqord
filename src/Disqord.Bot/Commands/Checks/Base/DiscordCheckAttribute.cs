using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

public abstract class DiscordCheckAttribute : CheckAttribute
{
    public abstract ValueTask<IResult> CheckAsync(IDiscordCommandContext context);

    public sealed override ValueTask<IResult> CheckAsync(ICommandContext context)
    {
        if (context is not IDiscordCommandContext discordContext)
            throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(IDiscordCommandContext)}.");

        return CheckAsync(discordContext);
    }
}
