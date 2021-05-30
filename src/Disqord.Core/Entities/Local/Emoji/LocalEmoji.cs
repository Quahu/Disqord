using System;

namespace Disqord
{
    public class LocalEmoji : ILocalConstruct, IEmoji
    {
        public static LocalEmoji Unicode(string unicode)
            => new(unicode);

        public static LocalCustomEmoji Custom(Snowflake id, string name = null, bool isAnimated = false)
            => new(id, name, isAnimated);

        public static LocalEmoji FromEmoji(IEmoji emoji)
        {
            if (emoji is ICustomEmoji customEmoji)
                return new LocalCustomEmoji(customEmoji.Id, customEmoji.Name, customEmoji.IsAnimated);

            return new LocalEmoji(emoji.Name);
        }

        public string Name { get; init; }

        public LocalEmoji()
        { }

        public LocalEmoji(string unicode)
        {
            Name = unicode;
        }

        public virtual bool Equals(IEmoji other)
            => Comparers.Emoji.Equals(this, other);

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Comparers.Emoji.GetHashCode(this);

        public override string ToString()
            => Name;

        public virtual LocalEmoji Clone()
            => MemberwiseClone() as LocalEmoji;

        object ICloneable.Clone()
            => Clone();

        public virtual void Validate()
        { }
    }
}
