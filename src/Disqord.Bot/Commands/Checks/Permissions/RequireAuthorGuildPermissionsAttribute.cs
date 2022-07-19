using System;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by authors with the given guild permissions.
/// </summary>
[Obsolete("Use RequireAuthorPermissionsAttribute instead.", true)]
public class RequireAuthorGuildPermissionsAttribute : RequireAuthorPermissionsAttribute
{
    public RequireAuthorGuildPermissionsAttribute(Permissions permissions)
        : base(permissions)
    { }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        throw new NotSupportedException();
    }
}
