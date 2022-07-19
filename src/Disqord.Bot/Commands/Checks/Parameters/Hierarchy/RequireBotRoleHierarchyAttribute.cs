using Disqord.Gateway;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the <see cref="IMember"/> parameter must be of lower guild hierarchy than the bot.
/// </summary>
public class RequireBotRoleHierarchyAttribute : RequireRoleHierarchyBaseAttribute
{
    protected override (string Name, IMember Member) GetTarget(IDiscordGuildCommandContext context)
    {
        var currentMember = context.Bot.GetCurrentMember(context.GuildId);
        if (currentMember == null)
            Throw.InvalidOperationException($"{nameof(RequireBotRoleHierarchyAttribute)} requires the current member cached.");

        return ("bot", currentMember);
    }
}
