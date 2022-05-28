using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

public abstract class DiscordParameterCheckAttribute : ParameterCheckAttribute
{
    protected DiscordParameterCheckAttribute()
    { }

    public abstract ValueTask<IResult> CheckAsync(IDiscordCommandContext context, IParameter parameter, object? argument);

    public sealed override ValueTask<IResult> CheckAsync(ICommandContext context, IParameter parameter, object? argument)
    {
        if (context is not IDiscordCommandContext discordContext)
            throw new InvalidOperationException($"The {GetType().Name} only accepts a {nameof(IDiscordCommandContext)}.");

        return CheckAsync(discordContext, parameter, argument);
    }
}
