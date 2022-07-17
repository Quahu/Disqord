using System;
using System.Threading.Tasks;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by authors with the given channel permissions.
/// </summary>
[Obsolete("Use RequireAuthorPermissionsAttribute instead.", true)]
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
        throw new NotSupportedException();
    }
}
