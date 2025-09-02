using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal static class JsonUtilities
{
    public const double MaxSafeInteger = 9007199254740991;

    private static readonly PropertyInfo _ignoreConditionProperty;
    private static readonly PropertyInfo _converterStrategyProperty;

    public static ulong ReadUInt64FromString(this ref Utf8JsonReader reader)
    {
        if (!reader.HasValueSequence)
        {
            return ulong.Parse(reader.ValueSpan);
        }

        if (reader.ValueSequence.IsSingleSegment)
        {
            return ulong.Parse(reader.ValueSequence.FirstSpan);
        }

        var buffer = (stackalloc byte[20]);
        reader.ValueSequence.CopyTo(buffer);
        return ulong.Parse(buffer[..(int) reader.ValueSequence.Length]);
    }

    public static void SetIgnoreCondition(this JsonPropertyInfo propertyInfo, JsonIgnoreCondition condition)
    {
        _ignoreConditionProperty.SetValue(propertyInfo, condition);
    }

    public static void SetConverterStrategy(this JsonConverter converter, byte converterStrategy)
    {
        if (!Enum.IsDefined(_converterStrategyProperty.PropertyType, converterStrategy))
        {
            Throw.ArgumentException("Invalid converter strategy.", nameof(converterStrategy));
        }

        _converterStrategyProperty.SetValue(converter, Enum.ToObject(_converterStrategyProperty.PropertyType, converterStrategy));
    }

    [StackTraceHidden]
    [DoesNotReturn]
    public static void RethrowJsonException(JsonException ex)
    {
        throw new JsonException(ex.Message, new JsonException(null, ex.InnerException));
    }

    static JsonUtilities()
    {
        var ignoreConditionProperty = typeof(JsonPropertyInfo).GetProperty("IgnoreCondition", BindingFlags.Instance | BindingFlags.NonPublic);
        var converterStrategyProperty = typeof(JsonConverter).GetProperty("ConverterStrategy", BindingFlags.Instance | BindingFlags.NonPublic);
        if (converterStrategyProperty == null || ignoreConditionProperty == null)
        {
            Throw.InvalidOperationException("The System.Text.Json version is not compatible with this resolver.");
        }

        _ignoreConditionProperty = ignoreConditionProperty;
        _converterStrategyProperty = converterStrategyProperty;
    }
}
