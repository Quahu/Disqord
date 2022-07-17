using System.Globalization;
using Disqord.Gateway;

namespace Disqord.Bot.Commands.Text;

internal sealed class DiscordTextGuildCommandContext : DiscordTextCommandContext, IDiscordTextGuildCommandContext
{
    public new IMember Author => (base.Author as IMember)!;

    public new Snowflake GuildId => base.GuildId!.Value;

    public IGuildChannel Channel { get; }

    public DiscordTextGuildCommandContext(
        DiscordBotBase bot,
        IPrefix prefix,
        IGatewayUserMessage message,
        IGuildChannel channel,
        CultureInfo locale,
        CultureInfo? guildLocale)
        : base(bot, prefix, message, locale, guildLocale)
    {
        Channel = channel;
    }
}
