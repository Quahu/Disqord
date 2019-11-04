using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Disqord.Rest;

namespace Disqord
{
    /// <summary>
    ///     Provides various Discord utilities.
    /// </summary>
    public static partial class Discord
    {
        internal static class Internal
        {
            internal static string GetAvatarUrl(RestWebhook webhook, ImageFormat format = default, int size = 2048)
                => webhook.AvatarHash != null
                    ? GetUserAvatarUrl(webhook.Id, webhook.AvatarHash, format, size)
                    : GetDefaultUserAvatarUrl(DefaultAvatarColor.Blurple);

            internal static string GetAvatarUrl(IUser user, ImageFormat format = default, int size = 2048)
                => user.AvatarHash != null
                    ? GetUserAvatarUrl(user.Id, user.AvatarHash, format, size)
                    : GetDefaultUserAvatarUrl(user.Discriminator);

            internal static CultureInfo CreateLocale(string locale)
                => CultureInfo.ReadOnly(new CultureInfo(locale));
        }

        public const int DEFAULT_MAX_PRESENCE_COUNT = 5000;

        /// <summary>
        ///     Represents the default Discord CDN url.
        /// </summary>
        public const string CDN_URL = "https://cdn.discordapp.com/";

        /// <summary>
        ///     Returns the url for a custom emoji.
        /// </summary>
        /// <param name="emojiId"> The custom emoji's id. </param>
        /// <param name="isAnimated"> Whether the custom emoji is animated or not. </param>
        /// <param name="size"> The size of the custom emoji. </param>
        /// <returns>
        ///     The url of the custom emoji.
        /// </returns>
        public static string GetCustomEmojiUrl(Snowflake emojiId, bool isAnimated, int size = 2048)
            => FormatImageUrl($"emoji/{emojiId}", isAnimated ? ImageFormat.Gif : ImageFormat.Png, size);

        /// <summary>
        ///     Returns the url for a guild's icon.
        /// </summary>
        /// <param name="guildId"> The guild's id. </param>
        /// <param name="iconHash"> The guild's icon hash. </param>
        /// <param name="format"> The format to use. </param>
        /// <param name="size"> The size of the guild icon. </param>
        /// <returns>
        ///     The url of the guild icon.
        /// </returns>
        public static string GetGuildIconUrl(Snowflake guildId, string iconHash, ImageFormat format = default, int size = 2048)
            => FormatImageUrl($"icons/{guildId}/{iconHash}", format != default ? format : ImageFormat.Png, size);

        /// <summary>
        ///     Returns the url for a guild's splash.
        /// </summary>
        /// <param name="guildId"> The guild's id. </param>
        /// <param name="splashHash"> The guild's splash hash. </param>
        /// <param name="format"> The format to use. </param>
        /// <param name="size"> The size of the guild splash. </param>
        /// <returns>
        ///     The url of the guild's splash.
        /// </returns>
        public static string GetGuildSplashUrl(Snowflake guildId, string splashHash, ImageFormat format = default, int size = 2048)
            => FormatImageUrl($"splashes/{guildId}/{splashHash}", format != default ? format : ImageFormat.Png, size);

        /// <summary>
        ///     Returns the url for a default user avatar.
        /// </summary>
        /// <param name="userDiscriminator"> The user's discriminator. </param>
        /// <returns>
        ///     The url of the default user avatar.
        /// </returns>
        public static string GetDefaultUserAvatarUrl(string userDiscriminator)
            => FormatImageUrl($"embed/avatars/{ushort.Parse(userDiscriminator) % 5}", ImageFormat.Png);

        /// <summary>
        ///     Returns the url for a default user avatar.
        /// </summary>
        /// <param name="color"> The avatar color to return. </param>
        /// <returns>
        ///     The url of the default user avatar.
        /// </returns>
        public static string GetDefaultUserAvatarUrl(DefaultAvatarColor color)
            => FormatImageUrl($"embed/avatars/{(byte) color}", ImageFormat.Png);

        /// <summary>
        ///     Returns the url for a default user avatar.
        /// </summary>
        /// <param name="color"> The avatar color to return. </param>
        /// <returns>
        ///     The url of the default user avatar.
        /// </returns>
        public static string GetDefaultUserAvatarUrl(byte color)
            => FormatImageUrl($"embed/avatars/{color}", ImageFormat.Png);

        /// <summary>
        ///     Returns the url for a user's avatar.
        /// </summary>
        /// <param name="userId"> The user's id. </param>
        /// <param name="avatarHash"> The user's avatar hash. </param>
        /// <param name="format"> The format to use. </param>
        /// <param name="size"> The size of the user's avatar. </param>
        /// <returns>
        ///     The url of the user's avatar.
        /// </returns>
        public static string GetUserAvatarUrl(Snowflake userId, string avatarHash, ImageFormat format = default, int size = 2048)
            => FormatImageUrl($"avatars/{userId}/{avatarHash}", format != default ? format : (avatarHash.StartsWith("a_") ? ImageFormat.Gif : ImageFormat.Png), size);

        /// <summary>
        ///     Returns the url for an application's icon url.
        /// </summary>
        /// <param name="applicationId"> The application's id. </param>
        /// <param name="iconHash"> The application's icon hash. </param>
        /// <param name="format"> The format to use. </param>
        /// <param name="size"> The size of the application's icon. </param>
        /// <returns>
        ///     The url of the application's icon.
        /// </returns>
        public static string GetApplicationIconUrl(Snowflake applicationId, string iconHash, ImageFormat format = default, int size = 2048)
            => FormatImageUrl($"app-icons/{applicationId}/{iconHash}", format != default ? format : ImageFormat.Png, size);

        private static string FormatImageUrl(string path, ImageFormat format, int size = 0)
        {
            if (size > 0 && (size < 16 || size > 2048 || (size & -size) != size))
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be a power of 2 between 16 and 2048.");

            return $"{CDN_URL}{path}.{GetImageFormatString(format)}{(size > 0 ? $"?size={size}" : "")}";
        }

        private static string GetImageFormatString(ImageFormat format)
        {
            return format switch
            {
                ImageFormat.Png => "png",
                ImageFormat.Jpg => "jpg",
                ImageFormat.WebP => "webp",
                ImageFormat.Gif => "gif",
                _ => throw new ArgumentOutOfRangeException(nameof(format), "The image format must be set."),
            };
        }

        public static string ToReactionFormat(IEmoji emoji)
        {
            if (emoji == null)
                throw new ArgumentNullException(nameof(emoji));

            return emoji is CustomEmoji customEmoji
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
                    : $"<{customEmoji.Name}:{customEmoji.Id}>"
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

        public static readonly Regex JumpLinkRegex = new Regex(
            @"^https?://(?:(ptb|canary)\.)?discordapp\.com/channels/(?<guild_id>([0-9]{15,21})|(@me))/(?<channel_id>[0-9]{15,21})/(?<message_id>[0-9]{15,21})/?$",
            RegexOptions.Compiled);

        internal static string GetSystemMessageContent(ISystemMessage message, IGuild guild)
        {
            return message.Type switch
            {
                SystemMessageType.RecipientAdded => $"{message.Author.Name} added {message.MentionedUsers[0].Name} to the group.",
                SystemMessageType.RecipientRemoved => $"{message.Author.Name} removed {message.MentionedUsers[0].Name} from the group.",
                // TODO
                SystemMessageType.Call => throw new NotImplementedException(),
                SystemMessageType.ChannelNameChanged => $"{message.Author.Name} changed the channel name: {message.RawContent}",
                SystemMessageType.ChannelIconChanged => $"{message.Author.Name} changed the channel icon.",
                SystemMessageType.ChannelMessagePinned => $"{message.Author.Name} pinned a message to this channel.",
                SystemMessageType.MemberJoined => string.Format(MemberJoinFormats[message.Id.CreatedAt.ToUnixTimeMilliseconds() % MemberJoinFormats.Length], message.Author.Name),
                SystemMessageType.GuildBoosted => $"{message.Author.Name} just boosted the server!",
                SystemMessageType.GuildBoostedFirstTier => $"{message.Author.Name} just boosted the server! {guild?.Name ?? "The server"} has achieved **Level 1!**",
                SystemMessageType.GuildBoostedSecondTier => $"{message.Author.Name} just boosted the server! {guild?.Name ?? "The server"} has achieved **Level 2!**",
                SystemMessageType.GuildBoostedThirdTier => $"{message.Author.Name} just boosted the server! {guild?.Name ?? "The server"} has achieved **Level 3!**",
                SystemMessageType.ChannelFollowed => $"{message.Author.Name} has added {message.RawContent} to this channel.",
                _ => string.Empty,
            };
        }

        internal static readonly string[] MemberJoinFormats =
        {
            "{0} just joined the server - glhf!",
            "{0} just joined. Everyone, look busy!",
            "{0} just joined. Can I get a heal?",
            "{0} joined your party.",
            "{0} joined. You must construct additional pylons.",
            "Ermagherd. {0} is here.",
            "Welcome, {0}. Stay awhile and listen.",
            "Welcome, {0}. We were expecting you ( ͡° ͜ʖ ͡°)",
            "Welcome, {0}. We hope you brought pizza.",
            "Welcome {0}. Leave your weapons by the door.",
            "A wild {0} appeared.",
            "Swoooosh. {0} just landed.",
            "Brace yourselves. {0} just joined the server.",
            "{0} just joined... or did they?",
            "{0} just arrived. Seems OP - please nerf.",
            "{0} just slid into the server.",
            "A {0} has spawned the server.",
            "Big {0} showed up!",
            "Where’s {0}? the server!",
            "{0} hopped into the server. Kangaroo!!",
            "{0} just showed up. Hold my beer.",
            "Challenger approaching - {0} has appeared!",
            "It's a bird! It's a plane! Nevermind, it's just {0}.",
            "It's {0}! Praise the sun! \\\\[T]/",
            "Never gonna give {0} up. Never gonna let {0} down.",
            "{0} has joined the battle bus.",
            "Cheers, love! {0}'s here!",
            "Hey! Listen! {0} has joined!",
            "We've been expecting you {0}",
            "It's dangerous to go alone, take {0}!",
            "{0} has joined the server! It's super effective!",
            "Cheers, love! {0} is here!",
            "{0} is here, as the prophecy foretold.",
            "{0} has arrived. Party's over.",
            "Ready player {0}",
            "{0} is here to kick butt and chew bubblegum. And {0} is all out of gum.",
            "Hello. Is it {0} you're looking for?",
            "{0} has joined. Stay a while and listen!",
            "Roses are red, violets are blue, {0} joined this server with you"
        };
    }
}