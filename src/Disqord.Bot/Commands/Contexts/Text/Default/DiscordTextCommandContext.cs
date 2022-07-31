using System;
using System.Collections.Generic;
using System.Globalization;
using Disqord.Gateway;
using Qmmands.Text;

namespace Disqord.Bot.Commands.Text;

internal class DiscordTextCommandContext : DiscordCommandContext<ITextCommand>, IDiscordTextCommandContext
{
    public override CultureInfo Locale { get; }

    public override CultureInfo? GuildLocale { get; }

    public ReadOnlyMemory<char>? InputString { get; set; }

    public ReadOnlyMemory<char> RawArgumentString { get; set; }

    public IEnumerable<ReadOnlyMemory<char>>? Path { get; set; }

    public bool IsOverloadDeterminant { get; set; }

    public override Snowflake ChannelId => Message.ChannelId;

    public override IUser Author => Message.Author;

    public IPrefix Prefix { get; }

    public IGatewayUserMessage Message { get; }

    public DiscordTextCommandContext(
        DiscordBotBase bot,
        IPrefix prefix,
        IGatewayUserMessage message,
        CultureInfo locale,
        CultureInfo? guildLocale)
        : base(bot)
    {
        Prefix = prefix;
        Message = message;
        Locale = locale;
        GuildLocale = guildLocale;
    }

    protected override Snowflake? GetGuildId()
    {
        return Message.GuildId;
    }
}
