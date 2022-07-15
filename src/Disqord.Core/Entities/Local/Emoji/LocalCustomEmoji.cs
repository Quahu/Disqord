using System;
using Qommon;

namespace Disqord
{
    public class LocalCustomEmoji : LocalEmoji, ICustomEmoji
    {
        public Snowflake Id { get; init; }

        public bool IsAnimated { get; init; }

        public string Tag => this.GetString();

        public LocalCustomEmoji()
        { }

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

        public override LocalCustomEmoji Clone()
            => MemberwiseClone() as LocalCustomEmoji;

        public static bool TryParse(string value, out LocalCustomEmoji result)
        {
            Guard.IsNotNull(value);

            return TryParse(value.AsSpan(), out result);
        }

        public static bool TryParse(ReadOnlySpan<char> value, out LocalCustomEmoji result)
        {
            result = null;
            if (value.Length < 21)
                return false;

            if (value[0] != '<' || value[^1] != '>')
                return false;

            value = value.Slice(1, value.Length - 2);
            var isAnimated = value[0] == 'a';
            if (value[isAnimated ? 1 : 0] != ':')
                return false;

            value = value.Slice(isAnimated ? 2 : 1);
            var colonIndex = value.IndexOf(':');
            if (colonIndex == -1)
                return false;

            var nameSpan = value.Slice(0, colonIndex);
            if (nameSpan.IsEmpty || nameSpan.Length > 32 || nameSpan.IsWhiteSpace())
                return false;

            var idSpan = value.Slice(colonIndex + 1);
            if (!Snowflake.TryParse(idSpan, out var id))
                return false;

            result = new LocalCustomEmoji(id, new string(nameSpan), isAnimated);
            return true;
        }
    }
}
