namespace Disqord.Bot
{
    public class RequireAuthorHierarchyAttribute : RequireHierarchyBaseAttribute
    {
        protected override (string Name, IMember Member) GetTarget(DiscordGuildCommandContext context)
            => ("author", context.Author);
    }
}
