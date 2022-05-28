using System;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed if the bot has given guild permissions.
/// </summary>
[Obsolete("Use RequireBotPermissionsAttribute instead.")]
public class RequireBotGuildPermissionsAttribute : RequireBotPermissionsAttribute
{
    public RequireBotGuildPermissionsAttribute(Permission permissions)
        : base(permissions)
    { }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context is not IDiscordGuildCommandContext guildContext)
            return Results.Success;

        var permissions = guildContext.Bot.GetMember(guildContext.GuildId, guildContext.Bot.CurrentUser.Id).GetPermissions();
        if (permissions.Has(Permissions))
            return Results.Success;

        return Results.Failure($"The bot lacks the necessary guild permissions ({Permissions & ~permissions}) to execute this.");
    }
}
