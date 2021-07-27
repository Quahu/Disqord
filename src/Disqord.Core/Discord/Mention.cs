using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Disqord
{
    public static class Mention
    {
        /// <summary>
        ///     Gets the <see cref="string"/> that, if present in a message, will mention all online users that can view the message.
        /// </summary>
        public const string Here = "@here";

        /// <summary>
        ///     Gets the <see cref="string"/> that, if present in a message, will mention all users that can view the message.
        /// </summary>
        public const string Everyone = "@everyone";

        public static string User(IMember member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            return User(member.Id, member.Nick != null);
        }

        public static string User(IUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return User(user.Id, user is IMember member && member.Nick != null);
        }

        public static string User(Snowflake id, bool hasNick = false)
            => hasNick ? $"<@!{id}>" : $"<@{id}>";

        public static string Channel(IGuildChannel channel)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));

            return Channel(channel.Id);
        }

        public static string Channel(Snowflake id)
            => $"<#{id}>";

        public static string Role(IRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Role(role.Id);
        }

        public static string Role(Snowflake id)
            => $"<@&{id}>";

        public static bool TryParseUser(string value, out Snowflake result)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return TryParseUser(value.AsSpan(), out result);
        }

        public static bool TryParseUser(ReadOnlySpan<char> value, out Snowflake result)
        {
            result = 0;
            return value.Length > 3 && value[0] == '<' && value[1] == '@' && value[^1] == '>'
                && Snowflake.TryParse(value[2] == '!' ? value[3..^1] : value[2..^1], out result);
        }

        public static bool TryParseChannel(string value, out Snowflake result)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return TryParseChannel(value.AsSpan(), out result);
        }

        public static bool TryParseChannel(ReadOnlySpan<char> value, out Snowflake result)
        {
            result = 0;
            return value.Length > 3
                && value[0] == '<'
                && value[1] == '#'
                && value[value.Length - 1] == '>'
                && Snowflake.TryParse(value[2..^1], out result);
        }

        public static bool TryParseRole(string value, out Snowflake result)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return TryParseRole(value.AsSpan(), out result);
        }

        public static bool TryParseRole(ReadOnlySpan<char> value, out Snowflake result)
        {
            result = 0;
            return value.Length > 3
                && value[0] == '<'
                && value[1] == '@'
                && value[2] == '&'
                && value[value.Length - 1] == '>'
                && Snowflake.TryParse(value[3..^1], out result);
        }

        public static IEnumerable<Snowflake> ParseUsers(string content)
        {
            var matches = Regex.Matches(content, "<@!?([0-9]+)>");
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (Snowflake.TryParse(match.Groups[1].Value, out var snowflake))
                    yield return snowflake;
            }
        }

        public static IEnumerable<Snowflake> ParseChannels(string content)
        {
            var matches = Regex.Matches(content, "<#([0-9]+)>");
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (Snowflake.TryParse(match.Groups[1].Value, out var snowflake))
                    yield return snowflake;
            }
        }

        public static IEnumerable<Snowflake> ParseRoles(string content)
        {
            var matches = Regex.Matches(content, "<@&([0-9])+>");
            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                if (Snowflake.TryParse(match.Groups[1].Value, out var snowflake))
                    yield return snowflake;
            }
        }

        public static string Escape(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return Regex.Replace(input, "@(everyone|here|[!&]?[0-9]{17,21})", x => $"@\u200b{x.Groups[1]}", RegexOptions.Compiled);
        }
    }
}
