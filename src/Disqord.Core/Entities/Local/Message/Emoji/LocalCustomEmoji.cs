using System;

namespace Disqord
{
    public class LocalCustomEmoji : LocalEmoji, ICustomEmoji
    {
        public Snowflake Id { get; }

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public bool IsAnimated { get; }

        public string Tag => this.GetMessageFormat();

        /// <summary>
        ///     Instantiates a new custom emoji with the specified custom emoji ID
        ///     and optionally a name and whether the emoji is animated.
        /// </summary>
        /// <remarks>
        ///     The optional parameters are purely for the developer's convenience and have
        ///     no effect on any Discord API interactions.
        /// </remarks>
        /// <param name="id"> The ID of this emoji. </param>
        /// <param name="name"> The name of this emoji. </param>
        /// <param name="isAnimated"> Whether this emoji is animated. </param>
        public LocalCustomEmoji(Snowflake id, string name = null, bool isAnimated = false)
            : base(name)
        {
            Id = id;
            IsAnimated = isAnimated;
        }

        public bool Equals(ICustomEmoji other)
            => Comparers.Emoji.Equals(this, other);

        public override string ToString()
            => Tag;

        public static bool TryParse(string value, out LocalCustomEmoji result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(value) || value.Length < 21)
                return false;

            var valueSpan = value.AsSpan();
            if (valueSpan[0] != '<' || valueSpan[^1] != '>')
                return false;

            valueSpan = valueSpan.Slice(1, valueSpan.Length - 2);
            var isAnimated = valueSpan[0] == 'a';
            if (valueSpan[isAnimated ? 1 : 0] != ':')
                return false;

            valueSpan = valueSpan.Slice(isAnimated ? 2 : 1);
            var colonIndex = valueSpan.IndexOf(':');
            if (colonIndex == -1)
                return false;

            var nameSpan = valueSpan.Slice(0, colonIndex);
            if (nameSpan.IsEmpty || nameSpan.Length > 32 || nameSpan.IsWhiteSpace())
                return false;

            var idSpan = valueSpan.Slice(colonIndex + 1);
            if (!Snowflake.TryParse(idSpan, out var id))
                return false;

            result = new LocalCustomEmoji(id, new string(nameSpan), isAnimated);
            return true;
        }
    }
}
