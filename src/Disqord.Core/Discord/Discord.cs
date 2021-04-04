using System;
using System.Text.RegularExpressions;

namespace Disqord
{
    /// <summary>
    ///     Provides various Discord utilities.
    /// </summary>
    public static partial class Discord
    {
        public static string GetReactionFormat(this IEmoji emoji)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return emoji is ICustomEmoji customEmoji
                ? $"{customEmoji.Name}:{customEmoji.Id}"
                : emoji.Name;
        }

        public static string GetMessageFormat(this IEmoji emoji)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return emoji is ICustomEmoji customEmoji
                ? customEmoji.IsAnimated
                    ? $"<a:{customEmoji.Name}:{customEmoji.Id}>"
                    : $"<:{customEmoji.Name}:{customEmoji.Id}>"
                : emoji.Name;
        }

        public static readonly Regex MessageJumpLinkRegex = new Regex(
            @"^https?://(?:(ptb|canary)\.)?discord(?:app)?\.com/channels/(?<guild_id>([0-9]{15,21})|(@me))/(?<channel_id>[0-9]{15,21})/(?<message_id>[0-9]{15,21})/?$",
            RegexOptions.Compiled);

        public static string MessageJumpLink(Snowflake? guildId, Snowflake channelId, Snowflake messageId) => guildId != null
            ? $"https://discord.com/channels/{guildId}/{channelId}/{messageId}"
            : $"https://discord.com/channels/@me/{channelId}/{messageId}";
    }
}