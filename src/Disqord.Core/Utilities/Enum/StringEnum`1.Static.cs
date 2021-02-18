using System.Runtime.CompilerServices;

namespace Disqord.Utilities
{
    public abstract partial class StringEnum<TEnum>
    {
        public static TEnum Create(string value)
            => new TEnum
            {
                Value = value
            };

        public static implicit operator string(StringEnum<TEnum> value)
            => value.Value;

        public static implicit operator StringEnum<TEnum>(string value)
            => Create(value);

        static StringEnum()
        {
            RuntimeHelpers.RunClassConstructor(typeof(TEnum).TypeHandle);
        }
    }
}
