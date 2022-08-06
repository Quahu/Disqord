using System.Globalization;

namespace Disqord.Bot.Commands.Application;

internal class DiscordApplicationCommandContext : DiscordCommandContext<ApplicationCommand>, IDiscordApplicationCommandContext
{
    public override CultureInfo Locale => Interaction.Locale;

    public override CultureInfo GuildLocale => Interaction.GuildLocale ?? CultureInfo.InvariantCulture;

    public IApplicationCommandInteraction Interaction { get; }

    public override IUser Author => Interaction.Author;

    public override Snowflake ChannelId => Interaction.ChannelId;

    public DiscordApplicationCommandContext(
        DiscordBotBase bot,
        IApplicationCommandInteraction interaction)
        : base(bot)
    {
        Interaction = interaction;
    }

    protected override Snowflake? GetGuildId()
    {
        return Interaction.GuildId;
    }
}
