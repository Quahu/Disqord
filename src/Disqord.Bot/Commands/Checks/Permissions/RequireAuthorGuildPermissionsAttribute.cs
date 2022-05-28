using System;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by authors with the given guild permissions.
/// </summary>
[Obsolete("Use RequireAuthorPermissionsAttribute instead.")]
public class RequireAuthorGuildPermissionsAttribute : RequireAuthorPermissionsAttribute
{
    public RequireAuthorGuildPermissionsAttribute(Permission permissions)
        : base(permissions)
    { }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context is not IDiscordGuildCommandContext guildContext)
            return Results.Success;

        var permissions = guildContext.Author.GetPermissions();
        if (permissions.Has(Permissions))
            return Results.Success;

        return Results.Failure($"You lack the necessary guild permissions ({Permissions & ~permissions}) to execute this.");
    }
}
