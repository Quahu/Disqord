using System;
using System.Threading.Tasks;
using Disqord.Bot.Commands.Application;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by authors with the given permissions.
/// </summary>
public class RequireAuthorPermissionsAttribute : DiscordCheckAttribute
{
    public Permission Permissions { get; }

    public RequireAuthorPermissionsAttribute(Permission permissions)
    {
        Permissions = permissions;
    }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context is not IDiscordGuildCommandContext guildContext)
            return Results.Success;

        Permission permissions;
        if (context is IDiscordApplicationCommandContext applicationContext)
        {
            permissions = applicationContext.AuthorPermissions;
        }
        else
        {
            if (guildContext.Bot.GetChannel(guildContext.GuildId, guildContext.ChannelId) is not IGuildChannel channel)
                throw new InvalidOperationException($"{nameof(RequireAuthorPermissionsAttribute)} requires the context channel.");

            // TODO: rework permissions
            permissions = guildContext.Author.GetPermissions().Flags | guildContext.Author.GetPermissions(channel).Flags;
        }

        if (permissions.HasFlag(Permissions))
            return Results.Success;

        return Results.Failure($"You lack the necessary permissions ({Permissions & ~permissions}) to execute this.");
    }
}
