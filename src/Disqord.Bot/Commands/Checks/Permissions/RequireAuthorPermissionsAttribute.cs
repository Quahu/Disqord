using System.Threading.Tasks;
using Disqord.Bot.Commands.Interaction;
using Disqord.Gateway;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by authors with the given permissions.
/// </summary>
public class RequireAuthorPermissionsAttribute : DiscordCheckAttribute
{
    /// <summary>
    ///     Gets the required permissions.
    /// </summary>
    public Permissions Permissions { get; }

    /// <summary>
    ///     Instantiates a new <see cref="RequireAuthorPermissionsAttribute"/>.
    /// </summary>
    /// <param name="permissions"> The required permissions. </param>
    public RequireAuthorPermissionsAttribute(Permissions permissions)
    {
        Permissions = permissions;
    }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context is not IDiscordGuildCommandContext guildContext)
            return Results.Success;

        Permissions permissions;
        if (context is IDiscordInteractionCommandContext interactionContext)
        {
            permissions = interactionContext.AuthorPermissions;
        }
        else
        {
            var channel = guildContext.Bot.GetChannel(guildContext.GuildId, guildContext.ChannelId) as IGuildChannel;
            if (channel == null)
                Throw.InvalidOperationException($"{nameof(RequireAuthorPermissionsAttribute)} requires the context channel.");

            permissions = guildContext.Author.CalculateChannelPermissions(channel);
        }

        if (permissions.HasFlag(Permissions))
            return Results.Success;

        return Results.Failure($"You lack the necessary permissions ({Permissions & ~permissions}) to execute this.");
    }
}
