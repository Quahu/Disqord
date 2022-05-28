namespace Disqord.Bot.Commands.Application;

internal sealed class DiscordApplicationGuildCommandContext : DiscordApplicationCommandContext, IDiscordApplicationGuildCommandContext
{
    public new IMember Author => (base.Author as IMember)!;

    public new Snowflake GuildId => base.GuildId!.Value;

    public DiscordApplicationGuildCommandContext(
        DiscordBotBase bot,
        IApplicationCommandInteraction interaction)
        : base(bot, interaction)
    { }
}
