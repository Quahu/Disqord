using System;

namespace Disqord
{
    public readonly partial struct Snowflake
    {
        TypeCode IConvertible.GetTypeCode()
            => RawValue.GetTypeCode();

        bool IConvertible.ToBoolean(IFormatProvider provider)
            => ((IConvertible) RawValue).ToBoolean(provider);

        byte IConvertible.ToByte(IFormatProvider provider)
            => ((IConvertible) RawValue).ToByte(provider);

        char IConvertible.ToChar(IFormatProvider provider)
            => ((IConvertible) RawValue).ToChar(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
            => ((IConvertible) RawValue).ToDateTime(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider)
            => ((IConvertible) RawValue).ToDecimal(provider);

        double IConvertible.ToDouble(IFormatProvider provider)
            => ((IConvertible) RawValue).ToDouble(provider);

        short IConvertible.ToInt16(IFormatProvider provider)
            => ((IConvertible) RawValue).ToInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider)
            => ((IConvertible) RawValue).ToInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider)
            => ((IConvertible) RawValue).ToInt64(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider)
            => ((IConvertible) RawValue).ToSByte(provider);

        float IConvertible.ToSingle(IFormatProvider provider)
            => ((IConvertible) RawValue).ToSingle(provider);

        string IConvertible.ToString(IFormatProvider provider)
            => RawValue.ToString(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            => ((IConvertible) RawValue).ToType(conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider)
            => ((IConvertible) RawValue).ToUInt16(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider)
            => ((IConvertible) RawValue).ToUInt32(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider)
            => ((IConvertible) RawValue).ToUInt64(provider);
    }
}
