using System;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed if the bot has given channel permissions.
/// </summary>
[Obsolete("Use RequireBotPermissionsAttribute instead.")]
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
        if (context is not IDiscordGuildCommandContext guildContext)
            return Results.Success;

        if (guildContext.Bot.GetChannel(guildContext.GuildId, context.ChannelId) is not IGuildChannel channel)
            throw new InvalidOperationException($"{nameof(RequireBotChannelPermissionsAttribute)} requires the context channel.");

        var permissions = guildContext.Bot.GetMember(guildContext.GuildId, guildContext.Bot.CurrentUser.Id).GetPermissions(channel);
        if (permissions.Has(Permissions))
            return Results.Success;

        return Results.Failure($"The bot lacks the necessary channel permissions ({Permissions & ~permissions}) to execute this.");
    }
}
