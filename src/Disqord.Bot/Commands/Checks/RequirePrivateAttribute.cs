using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed in private (e.g. direct) channels.
/// </summary>
public class RequirePrivateAttribute : DiscordCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="RequirePrivateAttribute"/>.
    /// </summary>
    public RequirePrivateAttribute()
    { }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context.GuildId == null)
            return Results.Success;

        return Results.Failure("This can only be executed outside of a guild.");
    }
}
