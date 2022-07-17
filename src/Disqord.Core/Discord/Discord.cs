using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Qommon;

namespace Disqord;

/// <summary>
///     Represents various Discord constants and utilities.
/// </summary>
public static partial class Discord
{
    /// <summary>
    ///     Gets the names of the locales supported by Discord.
    ///     See <a href="https://discord.com/developers/docs/reference#locales">Discord documentation.</a>
    /// </summary>
    public static IReadOnlyList<string> LocaleNames { get; } = new[]
    {
        "da",
        "de",
        "en-GB",
        "en-US",
        "es-ES",
        "fr",
        "hr",
        "it",
        "lt",
        "hu",
        "nl",
        "no",
        "pl",
        "pt-BR",
        "ro",
        "fi",
        "sv-SE",
        "vi",
        "tr",
        "cs",
        "el",
        "bg",
        "ru",
        "uk",
        "hi",
        "th",
        "zh-CN",
        "ja",
        "zh-TW",
        "ko"
    };

    /// <summary>
    ///     Gets the locales supported by Discord.
    /// </summary>
    public static IEnumerable<CultureInfo> Locales
    {
        get
        {
            var names = LocaleNames;
            var nameCount = names.Count;
            for (var i = 0; i < nameCount; i++)
            {
                var name = names[i];
                yield return CultureInfo.GetCultureInfo(name);
            }
        }
    }

    public static string GetReactionFormat(this IEmoji emoji)
    {
        Guard.IsNotNull(emoji);

        if (emoji is ICustomEmoji customEmoji)
            return $"{customEmoji.Name ?? "_"}:{customEmoji.Id}";

        Guard.IsNotNull(emoji.Name);

        return emoji.Name;
    }

    public static readonly Regex MessageJumpLinkRegex = new(@"^https?://(?:(ptb|canary)\.)?discord(?:app)?\.com/channels/(?<guild_id>([0-9]{15,21})|(@me))/(?<channel_id>[0-9]{15,21})/(?<message_id>[0-9]{15,21})/?$");

    public static string MessageJumpLink(Snowflake? guildId, Snowflake channelId, Snowflake messageId)
    {
        return guildId != null
            ? $"https://discord.com/channels/{guildId}/{channelId}/{messageId}"
            : $"https://discord.com/channels/@me/{channelId}/{messageId}";
    }
}
