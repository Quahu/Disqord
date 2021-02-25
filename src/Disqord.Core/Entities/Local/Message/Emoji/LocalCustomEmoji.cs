using System;
using System.Diagnostics.CodeAnalysis;

namespace Disqord
{
    public class LocalCustomEmoji : LocalEmoji, ICustomEmoji
    {
        public Snowflake Id { get; }

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public bool IsAnimated { get; }

        public string Tag => this.GetMessageFormat();

        public LocalCustomEmoji(Snowflake id, string name, bool isAnimated = false)
            : base(name)
        {
            Id = id;
            IsAnimated = isAnimated;
        }

        public bool Equals(ICustomEmoji? other)
            => Discord.Comparers.Emoji.Equals(this, other);

        public override string ToString()
            => Tag;

        public static bool TryParse(string value, [MaybeNullWhen(false)] out LocalCustomEmoji result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(value) || value.Length < 21)
                return false;

            var valueSpan = value.AsSpan();
            if (valueSpan[0] != '<' || valueSpan[valueSpan.Length - 1] != '>')
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