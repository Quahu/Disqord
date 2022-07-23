using System;

namespace Disqord;

public readonly partial struct Snowflake
{
    TypeCode IConvertible.GetTypeCode()
    {
        return TypeCode.UInt64;
    }

    bool IConvertible.ToBoolean(IFormatProvider? provider)
    {
        return Convert.ToBoolean(RawValue, provider);
    }

    byte IConvertible.ToByte(IFormatProvider? provider)
    {
        return Convert.ToByte(RawValue, provider);
    }

    char IConvertible.ToChar(IFormatProvider? provider)
    {
        return Convert.ToChar(RawValue, provider);
    }

    DateTime IConvertible.ToDateTime(IFormatProvider? provider)
    {
        return Convert.ToDateTime(RawValue, provider);
    }

    decimal IConvertible.ToDecimal(IFormatProvider? provider)
    {
        return Convert.ToDecimal(RawValue, provider);
    }

    double IConvertible.ToDouble(IFormatProvider? provider)
    {
        return Convert.ToDouble(RawValue, provider);
    }

    short IConvertible.ToInt16(IFormatProvider? provider)
    {
        return Convert.ToInt16(RawValue, provider);
    }

    int IConvertible.ToInt32(IFormatProvider? provider)
    {
        return Convert.ToInt32(RawValue, provider);
    }

    long IConvertible.ToInt64(IFormatProvider? provider)
    {
        return Convert.ToInt64(RawValue, provider);
    }

    sbyte IConvertible.ToSByte(IFormatProvider? provider)
    {
        return Convert.ToSByte(RawValue, provider);
    }

    float IConvertible.ToSingle(IFormatProvider? provider)
    {
        return Convert.ToSingle(RawValue, provider);
    }

    string IConvertible.ToString(IFormatProvider? provider)
    {
        return RawValue.ToString(provider);
    }

    object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
    {
        return Convert.ChangeType(RawValue, conversionType, provider);
    }

    ushort IConvertible.ToUInt16(IFormatProvider? provider)
    {
        return Convert.ToUInt16(RawValue, provider);
    }

    uint IConvertible.ToUInt32(IFormatProvider? provider)
    {
        return Convert.ToUInt32(RawValue, provider);
    }

    ulong IConvertible.ToUInt64(IFormatProvider? provider)
    {
        return Convert.ToUInt64(RawValue, provider);
    }
}
