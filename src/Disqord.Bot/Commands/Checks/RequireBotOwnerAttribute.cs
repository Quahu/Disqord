using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by the bot owners.
/// </summary>
public class RequireBotOwnerAttribute : DiscordCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="RequireBotOwnerAttribute"/>.
    /// </summary>
    public RequireBotOwnerAttribute()
    { }

    /// <inheritdoc/>
    public override async ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (await context.Bot.IsOwnerAsync(context.Author.Id).ConfigureAwait(false))
            return Results.Success;

        return Results.Failure("This can only be executed by the bot owners.");
    }
}
