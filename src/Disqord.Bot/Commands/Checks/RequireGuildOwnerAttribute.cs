using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by the guild owner.
/// </summary>
public class RequireGuildOwnerAttribute : DiscordGuildCheckAttribute
{
    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context)
    {
        var guild = context.Bot.GetGuild(context.GuildId);
        if (context.AuthorId == guild.OwnerId)
            return Results.Success;

        return Results.Failure("This can only be executed by the guild owner.");
    }
}
