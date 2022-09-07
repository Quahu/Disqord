using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by the guild owner.
/// </summary>
public class RequireGuildOwnerAttribute : DiscordGuildCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="RequireGuildOwnerAttribute"/>.
    /// </summary>
    public RequireGuildOwnerAttribute()
    { }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context)
    {
        var guild = context.Bot.GetGuild(context.GuildId);
        if (guild == null)
            Throw.InvalidOperationException($"{nameof(RequireGuildOwnerAttribute)} requires the context guild cached.");

        if (context.AuthorId == guild.OwnerId)
            return Results.Success;

        return Results.Failure("This can only be executed by the guild owner.");
    }
}
