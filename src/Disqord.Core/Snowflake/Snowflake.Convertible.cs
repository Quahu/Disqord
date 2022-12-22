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
        return ((IConvertible) RawValue).ToBoolean(provider);
    }

    byte IConvertible.ToByte(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToByte(provider);
    }

    char IConvertible.ToChar(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToChar(provider);
    }

    DateTime IConvertible.ToDateTime(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToDateTime(provider);
    }

    decimal IConvertible.ToDecimal(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToDecimal(provider);
    }

    double IConvertible.ToDouble(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToDouble(provider);
    }

    short IConvertible.ToInt16(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToInt16(provider);
    }

    int IConvertible.ToInt32(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToInt32(provider);
    }

    long IConvertible.ToInt64(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToInt64(provider);
    }

    sbyte IConvertible.ToSByte(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToSByte(provider);
    }

    float IConvertible.ToSingle(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToSingle(provider);
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
        return ((IConvertible) RawValue).ToUInt16(provider);
    }

    uint IConvertible.ToUInt32(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToUInt32(provider);
    }

    ulong IConvertible.ToUInt64(IFormatProvider? provider)
    {
        return ((IConvertible) RawValue).ToUInt64(provider);
    }
}
