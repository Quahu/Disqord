using System;

namespace Disqord
{
    public sealed class LocalCustomEmoji : ICustomEmoji
    {
        public Snowflake Id { get; }

        public string Name { get; }

        public bool IsAnimated { get; }

        public string ReactionFormat => Discord.ToReactionFormat(this);

        public string MessageFormat => Discord.ToMessageFormat(this);

        public string Tag => MessageFormat;

        public LocalCustomEmoji(Snowflake id, string name = null, bool isAnimated = false)
        {
            Id = id;
            Name = name;
            IsAnimated = isAnimated;
        }

        public string GetUrl(int size = 2048)
            => Discord.Cdn.GetCustomEmojiUrl(Id, IsAnimated, size);

        public bool Equals(IEmoji other)
            => Discord.Comparers.Emoji.Equals(this, other);

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Discord.Comparers.Emoji.GetHashCode(this);

        public override string ToString()
            => MessageFormat;

        public static bool TryParse(string value, out LocalCustomEmoji result)
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