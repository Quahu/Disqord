using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Qommon;
using Qommon.Serialization;

namespace Disqord.Serialization.Json.System;

internal class JsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    private static readonly PropertyInfo _ignoreConditionProperty;

    private static readonly ConditionalWeakTable<Type, JsonConverter> _optionalConverters = new();
    private static readonly ConditionalWeakTable<Type, JsonConverter> _snowflakeDictionaryConverters = new();
    private static readonly StreamConverter? _streamConverter = new();
    private static readonly JsonStringEnumConverter? _stringEnumConverter = new();
    private static readonly SnowflakeConverter? _snowflakeConverter = new();
    private static readonly NullableConverter<Snowflake>? _nullableSnowflakeConverter = new(_snowflakeConverter);

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        // new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        var jsonTypeInfo = base.GetTypeInfo(type, options);

        var jsonProperties = jsonTypeInfo.Properties;
        var jsonPropertyCount = jsonProperties.Count;
        List<JsonPropertyInfo>? jsonPropertiesToRemove = null;
        for (var i = 0; i < jsonPropertyCount; i++)
        {
            var jsonProperty = jsonProperties[i];
            var fieldInfo = jsonProperty.AttributeProvider as FieldInfo;
            if (fieldInfo == null)
            {
                // TODO: IsExtensionData
                (jsonPropertiesToRemove ??= new()).Add(jsonProperty);
                continue;
            }

            var attributes = fieldInfo.GetCustomAttributes();
            JsonPropertyAttribute? jsonPropertyAttribute = null;
            foreach (var attribute in attributes)
            {
                if (attribute is JsonIgnoreAttribute)
                {
                    jsonPropertyAttribute = null;
                    break;
                }

                if (jsonPropertyAttribute == null)
                {
                    if (attribute is JsonPropertyAttribute)
                    {
                        jsonPropertyAttribute = Unsafe.As<JsonPropertyAttribute>(attribute);
                    }
                }
            }

            if (jsonPropertyAttribute == null)
            {
                (jsonPropertiesToRemove ??= new()).Add(jsonProperty);
                continue;
            }

            jsonProperty.Name = jsonPropertyAttribute.Name;

            if (typeof(IOptional).IsAssignableFrom(jsonProperty.PropertyType))
            {
                if (jsonProperty.PropertyType.GenericTypeArguments.Length == 0)
                {
                    Throw.InvalidOperationException($"JSON property type {jsonProperty.PropertyType} is not supported.");
                }

                _ignoreConditionProperty.SetValue(jsonProperty, JsonIgnoreCondition.WhenWritingDefault);

                jsonProperty.CustomConverter = GetOptionalConverter(jsonProperty.PropertyType);
            }
            else
            {
                if (jsonPropertyAttribute.NullValueHandling == NullValueHandling.Ignore)
                {
                    _ignoreConditionProperty.SetValue(jsonProperty, JsonIgnoreCondition.WhenWritingNull);
                }

                jsonProperty.CustomConverter = GetConverter(jsonProperty.PropertyType);
            }
        }

        if (jsonPropertiesToRemove != null)
        {
            foreach (var jsonProperty in jsonPropertiesToRemove)
                jsonTypeInfo.Properties.Remove(jsonProperty);
        }

        return jsonTypeInfo;
    }

    private static JsonConverter GetOptionalConverter(Type type)
    {
        var optionalType = type.GenericTypeArguments[0];
        return _optionalConverters.GetValue(optionalType, static type =>
        {
            var valueConverter = GetConverter(type);
            if (valueConverter != null)
            {
                var optionalConverterType = typeof(OptionalConverterWithValueConverter<>).MakeGenericType(type);
                return (Activator.CreateInstance(optionalConverterType, valueConverter) as JsonConverter)!;
            }
            else
            {
                var optionalConverterType = typeof(OptionalConverter<>).MakeGenericType(type);
                return (Activator.CreateInstance(optionalConverterType) as JsonConverter)!;
            }
        });
    }

    private static JsonConverter? GetConverter(Type type)
    {
        if (typeof(Stream).IsAssignableFrom(type))
        {
            return _streamConverter;
        }

        if (typeof(IJsonNode).IsAssignableFrom(type) && !typeof(JsonModel).IsAssignableFrom(type))
        {
            return (Activator.CreateInstance(typeof(JsonNodeConverter<>).MakeGenericType(type)) as JsonConverter)!;
        }

        if (!type.IsClass)
        {
            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
                type = nullableType;

            if (type.IsEnum)
            {
                var stringEnumAttribute = type.GetCustomAttribute<StringEnumAttribute>();
                if (stringEnumAttribute != null)
                {
                    return _stringEnumConverter;
                }
            }
            else if (type == typeof(Snowflake))
            {
                if (nullableType != null)
                {
                    return _nullableSnowflakeConverter;
                }

                return _snowflakeConverter;
            }
        }
        else
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var generics = type.GetGenericArguments();
                if (generics[0] == typeof(Snowflake))
                {
                    return _snowflakeDictionaryConverters.GetValue(generics[1], type => (Activator.CreateInstance(typeof(SnowflakeDictionaryConverter<>).MakeGenericType(type)) as JsonConverter)!);
                }
            }
        }

        return null;
    }

    static JsonTypeInfoResolver()
    {
        var ignoreConditionProperty = typeof(JsonPropertyInfo).GetProperty("IgnoreCondition", BindingFlags.Instance | BindingFlags.NonPublic);
        if (ignoreConditionProperty == null)
        {
            Throw.InvalidOperationException("The System.Text.Json version is not compatible with this resolver.");
        }

        _ignoreConditionProperty = ignoreConditionProperty;
    }
}
