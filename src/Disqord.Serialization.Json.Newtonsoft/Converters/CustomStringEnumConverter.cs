using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Disqord.Serialization.Json.Newtonsoft;

internal sealed class CustomStringEnumConverter : JsonConverter
{
    private static readonly Dictionary<Type, EnumInfo> EnumInfoCache = new();

    public override bool CanConvert(Type objectType)
    {
        var type = Nullable.GetUnderlyingType(objectType) ?? objectType;
        return type.IsEnum;
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var type = Nullable.GetUnderlyingType(objectType) ?? objectType;
        if (!type.IsEnum)
            throw new JsonSerializationException(isDeserialize: true, type, new InvalidOperationException($"Type {type.FullName} is not an enum."));

        if (reader.TokenType == JsonToken.Null)
            return null;

        var info = GetEnumInfo(type);

        if (reader.TokenType == JsonToken.Integer)
        {
            var value = Convert.ToUInt64(reader.Value!);
            return Enum.ToObject(type, value);
        }

        if (reader.TokenType == JsonToken.String)
        {
            var text = reader.Value!.ToString()!;

            if (Enum.TryParse(type, text, true, out var result))
                return result;

            if (info.NamesToValues.TryGetValue(text, out var value))
                return Enum.ToObject(type, value);

            throw new JsonSerializationException(isDeserialize: true, type, new InvalidOperationException($"Could not convert string '{text}' to enum type {type.FullName}."));
        }

        throw new JsonSerializationException(isDeserialize: true, type, new InvalidOperationException($"Unexpected token {reader.TokenType} when parsing enum."));
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var type = value.GetType();
        var info = GetEnumInfo(type);
        var enumValue = Convert.ToUInt64(value);

        if (info.ValuesToNames.TryGetValue(enumValue, out var name))
        {
            writer.WriteValue(name);
        }
        else
        {
            writer.WriteValue(enumValue);
        }
    }

    private static EnumInfo GetEnumInfo(Type type)
    {
        lock (EnumInfoCache)
        {
            if (EnumInfoCache.TryGetValue(type, out var info))
                return info;

            info = new EnumInfo(type);
            EnumInfoCache[type] = info;
            return info;
        }
    }

    private sealed class EnumInfo
    {
        public Dictionary<string, ulong> NamesToValues { get; }
        public Dictionary<ulong, string> ValuesToNames { get; }

        public EnumInfo(Type type)
        {
            NamesToValues = new Dictionary<string, ulong>(StringComparer.Ordinal);
            ValuesToNames = new Dictionary<ulong, string>();

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                var value = Convert.ToUInt64(field.GetValue(null)!);
                var name = field.Name;

                var enumMember = field.GetCustomAttribute<EnumMemberAttribute>();
                var serializedName = enumMember?.Value ?? name;

                NamesToValues[serializedName] = value;
                ValuesToNames[value] = serializedName;
            }
        }
    }
}
