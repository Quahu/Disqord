namespace Disqord.Bot
{
    [RequireGuild]
    public abstract class DiscordGuildModuleBase<TContext> : DiscordModuleBase<TContext>
        where TContext : DiscordGuildCommandContext
    { }
}
