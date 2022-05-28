using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the <see cref="IUser"/> parameter must not be a bot.
/// </summary>
public class RequireNotBotAttribute : DiscordParameterCheckAttribute
{
    public RequireNotBotAttribute()
    { }

    public override bool CanCheck(IParameter parameter, object? value)
    {
        return value is IUser;
    }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context, IParameter parameter, object? argument)
    {
        var user = (argument as IUser)!;
        if (!user.IsBot)
            return Results.Success;

        return Results.Failure("The provided user argument must not be a bot user.");
    }
}
