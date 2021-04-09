namespace Disqord.Bot
{
    public class RequireBotHierarchyAttribute : RequireHierarchyBaseAttribute
    {
        protected override (string Name, IMember Member) GetTarget(DiscordGuildCommandContext context)
            => ("bot", context.CurrentMember);
    }
}
