using System;
using System.Text;

namespace Disqord
{
    /// <summary>
    ///     Represents a bot account token.
    /// </summary>
    public sealed class BotToken : Token
    {
        /// <summary>
        ///     Represents the bot token epoch used for the creation date.
        /// </summary>
        public const int EPOCH = 1293840000;

        /// <summary>
        ///     Gets the ID of this bot token.
        /// </summary>
        public Snowflake Id { get; }

        /// <summary>
        ///     Gets the creation date of this bot token.
        /// </summary>
        public DateTimeOffset CreatedAt { get; }

        /// <summary>
        ///     Gets the cryptographic part of this bot token. Not useful in any way on its own.
        /// </summary>
        public string Hmac { get; }

        /// <summary>
        ///     Instantiates a new <see cref="BotToken"/>.
        /// </summary>
        /// <param name="value"> The bot token. </param>
        public BotToken(string value)
            : base(value)
        {
            // Example bot token: MjM4NDk0NzU2NTIxMzc3Nzky.CunGFQ.wUILz7z6HoJzVeq6pyHPmVgQgV4
            // Contains 3 parts separated by periods.
            var split = value.Split('.');
            if (split.Length != 3)
                throw new FormatException("The provided token is not a valid bot token.");

            // Converts the first segment from base64 to a snowflake, which is the ID of the bot.
            var userIdString = Encoding.UTF8.GetString(FromBase64(split[0]));
            if (!Snowflake.TryParse(userIdString, out var id))
                throw new FormatException("The provided token contains a malformed ID segment.");

            Id = id;

            // Converts the second segment from base64 to a number, which is the timestamp of the token
            // and offsets it by the const EPOCH.
            // TODO: logic
            CreatedAt = DateTimeOffset.UtcNow;

            // HMAC is just passtrough as it's not of any use.
            Hmac = split[2];
        }

        private static byte[] FromBase64(string segment)
        {
            // We fix Discord's base64 url-safe segments to work with Convert.FromBase64String().
            segment = segment.Replace('_', '/').Replace('-', '+');
            segment = (segment.Length % 4) switch
            {
                2 => segment + "==",
                3 => segment + "=",
                _ => segment
            };

            return Convert.FromBase64String(segment);
        }

        /// <inheritdoc/>
        public override string GetAuthorization()
            => $"Bot {RawValue}";
    }
}
