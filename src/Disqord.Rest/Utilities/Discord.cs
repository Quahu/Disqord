using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Disqord
{
    /// <summary>
    ///     Provides various Discord utilities.
    /// </summary>
    public static partial class Discord
    {
        public const int DEFAULT_MAX_PRESENCE_COUNT = 5000;

        public static string ToReactionFormat(IEmoji emoji)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return emoji is ICustomEmoji customEmoji
                ? $"{customEmoji.Name}:{customEmoji.Id}"
                : emoji.Name;
        }

        public static string ToMessageFormat(IEmoji emoji)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return emoji is ICustomEmoji customEmoji
                ? customEmoji.IsAnimated
                    ? $"<a:{customEmoji.Name}:{customEmoji.Id}>"
                    : $"<:{customEmoji.Name}:{customEmoji.Id}>"
                : emoji.Name;
        }

        internal static readonly Regex UserMentionsRegex = new Regex("<@!?([0-9]+)>", RegexOptions.Compiled);
        internal static readonly Regex ChannelMentionsRegex = new Regex("<#([0-9]+)>", RegexOptions.Compiled);
        internal static readonly Regex RoleMentionsRegex = new Regex("<@&([0-9])+>", RegexOptions.Compiled);

        public static string MentionUser(IMember member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            return MentionUser(member.Id, member.Nick != null);
        }

        public static string MentionUser(IUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return MentionUser(user.Id);
        }

        public static string MentionUser(Snowflake id, bool hasNick = false)
            => hasNick ? $"<@!{id}>" : $"<@{id}>";

        public static string MentionChannel(ITextChannel channel)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            return MentionChannel(channel.Id);
        }

        public static string MentionChannel(Snowflake id)
            => $"<#{id}>";

        public static string MentionRole(IRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return MentionRole(role.Id);
        }

        public static string MentionRole(Snowflake id)
            => $"<@&{id}>";

        public static bool TryParseUserMention(string value, out Snowflake result)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return TryParseUserMention(value.AsSpan(), out result);
        }

        public static bool TryParseUserMention(ReadOnlySpan<char> value, out Snowflake result)
        {
            result = 0;
            return value.Length > 3
                && value[0] == '<'
                && value[1] == '@'
                && value[value.Length - 1] == '>'
                && Snowflake.TryParse(value[2] == '!' ? value.Slice(3, value.Length - 4) : value.Slice(2, value.Length - 3), out result);
        }

        public static bool TryParseChannelMention(string value, out Snowflake result)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return TryParseChannelMention(value.AsSpan(), out result);
        }

        public static bool TryParseChannelMention(ReadOnlySpan<char> value, out Snowflake result)
        {
            result = 0;
            return value.Length > 3
                && value[0] == '<'
                && value[1] == '#'
                && value[value.Length - 1] == '>'
                && Snowflake.TryParse(value.Slice(2, value.Length - 3), out result);
        }

        public static bool TryParseRoleMention(string value, out Snowflake result)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return TryParseRoleMention(value.AsSpan(), out result);
        }

        public static bool TryParseRoleMention(ReadOnlySpan<char> value, out Snowflake result)
        {
            result = 0;
            return value.Length > 3
                && value[0] == '<'
                && value[1] == '@'
                && value[2] == '&'
                && value[value.Length - 1] == '>'
                && Snowflake.TryParse(value.Slice(3, value.Length - 4), out result);
        }

        public static IEnumerable<Snowflake> ParseUserMentions(IUserMessage message)
        {
            var matches = UserMentionsRegex.Matches(message.Content);
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (Snowflake.TryParse(match.Groups[1].Value, out var snowflake))
                    yield return snowflake;
            }
        }

        public static IEnumerable<Snowflake> ParseChannelMentions(IUserMessage message)
        {
            var matches = ChannelMentionsRegex.Matches(message.Content);
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (Snowflake.TryParse(match.Groups[1].Value, out var snowflake))
                    yield return snowflake;
            }
        }

        public static IEnumerable<Snowflake> ParseRoleMentions(IUserMessage message)
        {
            var matches = RoleMentionsRegex.Matches(message.Content);
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (Snowflake.TryParse(match.Groups[1].Value, out var snowflake))
                    yield return snowflake;
            }
        }

        private static readonly Regex MentionEscapeRegex = new Regex("@(everyone|here|[!&]?[0-9]{17,21})", RegexOptions.Compiled);

        public static string EscapeMentions(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return MentionEscapeRegex.Replace(input, x => $"@\u200b{x.Groups[1]}");
        }

        public static readonly Regex MessageJumpLinkRegex = new Regex(
            @"^https?://(?:(ptb|canary)\.)?discordapp\.com/channels/(?<guild_id>([0-9]{15,21})|(@me))/(?<channel_id>[0-9]{15,21})/(?<message_id>[0-9]{15,21})/?$",
            RegexOptions.Compiled);

        public static string MessageJumpLink(Snowflake? guildId, Snowflake channelId, Snowflake messageId) => guildId != null
            ? $"https://discordapp.com/channels/{guildId}/{channelId}/{messageId}"
            : $"https://discordapp.com/channels/@me/{channelId}/{messageId}";
    }
}