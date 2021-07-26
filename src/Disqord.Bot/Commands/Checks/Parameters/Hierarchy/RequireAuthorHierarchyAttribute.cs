namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the <see cref="IMember"/> parameter must be of lower guild hierarchy than the author.
    /// </summary>
    public class RequireAuthorHierarchyAttribute : RequireHierarchyBaseAttribute
    {
        protected override (string Name, IMember Member) GetTarget(DiscordGuildCommandContext context)
            => ("author", context.Author);
    }
}
