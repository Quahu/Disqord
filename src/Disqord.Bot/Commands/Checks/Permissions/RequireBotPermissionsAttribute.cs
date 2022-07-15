using System;
using System.Threading.Tasks;
using Disqord.Bot.Commands.Interaction;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed if the bot has given guild permissions.
/// </summary>
public class RequireBotPermissionsAttribute : DiscordCheckAttribute
{
    public Permission Permissions { get; }

    public RequireBotPermissionsAttribute(Permission permissions)
    {
        Permissions = permissions;
    }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context is not IDiscordGuildCommandContext guildContext)
            return Results.Success;

        Permission permissions;
        if (context is IDiscordInteractionCommandContext interactionContext)
        {
            permissions = interactionContext.ApplicationPermissions;
        }
        else
        {
            if (guildContext.Bot.GetChannel(guildContext.GuildId, guildContext.ChannelId) is not IGuildChannel channel)
                throw new InvalidOperationException($"{nameof(RequireBotPermissionsAttribute)} requires the context channel.");

            // TODO: rework permissions
            var currentMember = guildContext.Bot.GetMember(guildContext.GuildId, guildContext.Bot.CurrentUser.Id);
            permissions = currentMember.GetPermissions().Flags | currentMember.GetPermissions(channel).Flags;
        }

        if (permissions.HasFlag(Permissions))
            return Results.Success;

        return Results.Failure($"The bot lacks the necessary permissions ({Permissions & ~permissions}) to execute this.");
    }
}
