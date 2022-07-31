using System.Globalization;
using Disqord.Gateway;

namespace Disqord.Bot.Commands.Text;

internal sealed class DiscordTextGuildCommandContext : DiscordTextCommandContext, IDiscordTextGuildCommandContext
{
    public new Snowflake GuildId => base.GuildId!.Value;

    public new IMember Author => (base.Author as IMember)!;

    public IMessageGuildChannel? Channel { get; }

    public DiscordTextGuildCommandContext(
        DiscordBotBase bot,
        IPrefix prefix,
        IGatewayUserMessage message,
        IMessageGuildChannel? channel,
        CultureInfo locale,
        CultureInfo? guildLocale)
        : base(bot, prefix, message, locale, guildLocale)
    {
        Channel = channel;
    }
}
