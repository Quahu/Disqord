using System;

namespace Disqord.Utilities
{
    public abstract partial class StringEnum<TEnum> : IEquatable<StringEnum<TEnum>>, IEquatable<string>
        where TEnum : StringEnum<TEnum>, new()
    {
        private protected string Value { get; private init; }

        protected StringEnum()
        { }

        public bool Equals(StringEnum<TEnum> other)
            => Value.Equals(other.Value);

        public bool Equals(string other)
            => Value.Equals(other);

        public override bool Equals(object obj) => obj switch
        {
            StringEnum<TEnum> stringEnum => Equals(stringEnum),
            string value => Equals(value),
            _ => false
        };

        public override int GetHashCode()
            => HashCode.Combine(typeof(TEnum), Value);

        public override string ToString()
            => Value;
    }
}
