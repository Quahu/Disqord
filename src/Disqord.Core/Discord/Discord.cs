using System.Text.RegularExpressions;
using Qommon;

namespace Disqord
{
    /// <summary>
    ///     Represents various Discord constants and utilities.
    /// </summary>
    public static partial class Discord
    {
        public static string GetReactionFormat(this IEmoji emoji)
        {
            Guard.IsNotNull(emoji);

            return emoji is ICustomEmoji customEmoji
                ? $"{customEmoji.Name ?? "_"}:{customEmoji.Id}"
                : emoji.Name;
        }

        public static string GetMessageFormat(this IEmoji emoji)
        {
            Guard.IsNotNull(emoji);

            return emoji is ICustomEmoji customEmoji
                ? customEmoji.IsAnimated
                    ? $"<a:{customEmoji.Name ?? "_"}:{customEmoji.Id}>"
                    : $"<:{customEmoji.Name ?? "_"}:{customEmoji.Id}>"
                : emoji.Name;
        }

        public static readonly Regex MessageJumpLinkRegex = new(@"^https?://(?:(ptb|canary)\.)?discord(?:app)?\.com/channels/(?<guild_id>([0-9]{15,21})|(@me))/(?<channel_id>[0-9]{15,21})/(?<message_id>[0-9]{15,21})/?$");

        public static string MessageJumpLink(Snowflake? guildId, Snowflake channelId, Snowflake messageId)
            => guildId != null
                ? $"https://discord.com/channels/{guildId}/{channelId}/{messageId}"
                : $"https://discord.com/channels/@me/{channelId}/{messageId}";
    }
}
