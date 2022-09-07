namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the <see cref="IMember"/> or <see cref="IRole"/> parameter
///     must be of lower guild hierarchy than the author's role hierarchy.
/// </summary>
public class RequireAuthorRoleHierarchyAttribute : RequireRoleHierarchyBaseAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="RequireAuthorRoleHierarchyAttribute"/>.
    /// </summary>
    public RequireAuthorRoleHierarchyAttribute()
    { }

    /// <inheritdoc/>
    protected override (string Name, IMember Member) GetTarget(IDiscordGuildCommandContext context)
    {
        return ("author", context.Author);
    }
}
