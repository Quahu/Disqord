using System;

namespace Disqord;

public readonly partial struct Snowflake
{
    TypeCode IConvertible.GetTypeCode()
        => TypeCode.UInt64;

    bool IConvertible.ToBoolean(IFormatProvider? provider)
        => Convert.ToBoolean(RawValue, provider);

    byte IConvertible.ToByte(IFormatProvider? provider)
        => Convert.ToByte(RawValue, provider);

    char IConvertible.ToChar(IFormatProvider? provider)
        => Convert.ToChar(RawValue, provider);

    DateTime IConvertible.ToDateTime(IFormatProvider? provider)
        => Convert.ToDateTime(RawValue, provider);

    decimal IConvertible.ToDecimal(IFormatProvider? provider)
        => Convert.ToDecimal(RawValue, provider);

    double IConvertible.ToDouble(IFormatProvider? provider)
        => Convert.ToDouble(RawValue, provider);

    short IConvertible.ToInt16(IFormatProvider? provider)
        => Convert.ToInt16(RawValue, provider);

    int IConvertible.ToInt32(IFormatProvider? provider)
        => Convert.ToInt32(RawValue, provider);

    long IConvertible.ToInt64(IFormatProvider? provider)
        => Convert.ToInt64(RawValue, provider);

    sbyte IConvertible.ToSByte(IFormatProvider? provider)
        => Convert.ToSByte(RawValue, provider);

    float IConvertible.ToSingle(IFormatProvider? provider)
        => Convert.ToSingle(RawValue, provider);

    string IConvertible.ToString(IFormatProvider? provider)
        => RawValue.ToString(provider);

    object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
        => Convert.ChangeType(RawValue, conversionType, provider);

    ushort IConvertible.ToUInt16(IFormatProvider? provider)
        => Convert.ToUInt16(RawValue, provider);

    uint IConvertible.ToUInt32(IFormatProvider? provider)
        => Convert.ToUInt32(RawValue, provider);

    ulong IConvertible.ToUInt64(IFormatProvider? provider)
        => Convert.ToUInt64(RawValue, provider);
}