using Disqord.Gateway;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the <see cref="IMember"/> or <see cref="IRole"/> parameter
///     must be of lower guild hierarchy than the bot's role hierarchy.
/// </summary>
public class RequireBotRoleHierarchyAttribute : RequireRoleHierarchyBaseAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="RequireBotRoleHierarchyAttribute"/>.
    /// </summary>
    public RequireBotRoleHierarchyAttribute()
    { }

    /// <inheritdoc/>
    protected override (string Name, IMember Member) GetTarget(IDiscordGuildCommandContext context)
    {
        var currentMember = context.Bot.GetCurrentMember(context.GuildId);
        if (currentMember == null)
            Throw.InvalidOperationException($"{nameof(RequireBotRoleHierarchyAttribute)} requires the current member cached.");

        return ("bot", currentMember);
    }
}
