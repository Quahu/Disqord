using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed if the bot has given guild permissions.
/// </summary>
[Obsolete("Use RequireBotPermissionsAttribute instead.", true)]
public class RequireBotGuildPermissionsAttribute : RequireBotPermissionsAttribute
{
    public RequireBotGuildPermissionsAttribute(Permissions permissions)
        : base(permissions)
    { }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        throw new NotImplementedException();
    }
}
