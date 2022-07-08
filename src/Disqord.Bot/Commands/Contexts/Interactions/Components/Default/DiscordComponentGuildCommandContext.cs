namespace Disqord.Bot.Commands.Components;

internal sealed class DiscordComponentGuildCommandContext : DiscordComponentCommandContext, IDiscordComponentGuildCommandContext
{
    public new IMember Author => (base.Author as IMember)!;

    public new Snowflake GuildId => base.GuildId!.Value;

    public DiscordComponentGuildCommandContext(
        DiscordBotBase bot,
        IUserInteraction interaction)
        : base(bot, interaction)
    { }
}
