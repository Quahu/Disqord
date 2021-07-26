namespace Disqord.Bot
{
    /// <summary>
    ///     Specifies that the <see cref="IMember"/> parameter must be of lower guild hierarchy than the bot.
    /// </summary>
    public class RequireBotHierarchyAttribute : RequireHierarchyBaseAttribute
    {
        protected override (string Name, IMember Member) GetTarget(DiscordGuildCommandContext context)
            => ("bot", context.CurrentMember);
    }
}
