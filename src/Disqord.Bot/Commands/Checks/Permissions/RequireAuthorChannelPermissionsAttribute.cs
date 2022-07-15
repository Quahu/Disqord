using System;
using System.Threading.Tasks;
using Disqord.Bot.Commands.Application;
using Disqord.Gateway;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by authors with the given channel permissions.
/// </summary>
[Obsolete("Use RequireAuthorPermissionsAttribute instead.")]
public class RequireAuthorChannelPermissionsAttribute : RequireAuthorPermissionsAttribute
{
    public RequireAuthorChannelPermissionsAttribute(Permission permissions)
        : base(permissions)
    {
        ChannelPermissions.Mask(permissions, out var remainingPermissions);
        if (remainingPermissions != Permission.None)
            Throw.ArgumentOutOfRangeException(nameof(permissions), $"The permissions specified for {GetType()} contain non-channel permissions: {remainingPermissions}.");
    }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context is not IDiscordGuildCommandContext guildContext)
            return Results.Success;

        ChannelPermissions permissions;
        if (context is IDiscordApplicationCommandContext applicationContext)
        {
            permissions = applicationContext.AuthorPermissions;
        }
        else
        {
            if (guildContext.Bot.GetChannel(guildContext.GuildId, guildContext.ChannelId) is not IGuildChannel channel)
                throw new InvalidOperationException($"{nameof(RequireAuthorChannelPermissionsAttribute)} requires the context channel.");

            permissions = guildContext.Author.GetPermissions(channel);
        }

        if (permissions.Has(Permissions))
            return Results.Success;

        return Results.Failure($"You lack the necessary channel permissions ({Permissions & ~permissions}) to execute this.");
    }
}
