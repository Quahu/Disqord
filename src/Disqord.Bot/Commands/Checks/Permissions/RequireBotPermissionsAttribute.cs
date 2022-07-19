﻿using System.Threading.Tasks;
using Disqord.Bot.Commands.Interaction;
using Disqord.Gateway;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed if the bot has given guild permissions.
/// </summary>
public class RequireBotPermissionsAttribute : DiscordCheckAttribute
{
    public Permissions Permissions { get; }

    public RequireBotPermissionsAttribute(Permissions permissions)
    {
        Permissions = permissions;
    }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context is not IDiscordGuildCommandContext guildContext)
            return Results.Success;

        Permissions permissions;
        if (context is IDiscordInteractionCommandContext interactionContext)
        {
            permissions = interactionContext.ApplicationPermissions;
        }
        else
        {
            var channel = guildContext.Bot.GetChannel(guildContext.GuildId, guildContext.ChannelId) as IGuildChannel;
            if (channel == null)
                Throw.InvalidOperationException($"{nameof(RequireBotPermissionsAttribute)} requires the context channel cached.");

            var currentMember = guildContext.Bot.GetCurrentMember(guildContext.GuildId);
            if (currentMember == null)
                Throw.InvalidOperationException($"{nameof(RequireBotPermissionsAttribute)} requires the current member cached.");

            permissions = currentMember.CalculateChannelPermissions(channel);
        }

        if (permissions.HasFlag(Permissions))
            return Results.Success;

        return Results.Failure($"The bot lacks the necessary permissions ({Permissions & ~permissions}) to execute this.");
    }
}
