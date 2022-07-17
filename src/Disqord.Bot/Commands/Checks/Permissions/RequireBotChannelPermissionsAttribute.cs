using System;
using System.Threading.Tasks;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed if the bot has given channel permissions.
/// </summary>
[Obsolete("Use RequireBotPermissionsAttribute instead.", true)]
public class RequireBotChannelPermissionsAttribute : RequireBotPermissionsAttribute
{
    public RequireBotChannelPermissionsAttribute(Permission permissions)
        : base(permissions)
    {
        ChannelPermissions.Mask(permissions, out var remainingPermissions);
        if (remainingPermissions != Permission.None)
            Throw.ArgumentOutOfRangeException(nameof(remainingPermissions), $"The permissions specified for {GetType()} contain non-channel permissions: {remainingPermissions}.");
    }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        throw new NotImplementedException();
    }
}
