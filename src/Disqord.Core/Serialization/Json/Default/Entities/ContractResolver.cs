using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Qommon.Collections.Synchronized;
using Qommon.Serialization;

namespace Disqord.Serialization.Json.Default;

internal sealed class ContractResolver : DefaultContractResolver
{
    private readonly DefaultJsonSerializer _serializer;
    // TODO: custom StringEnumConverter
    private readonly StringEnumConverter _stringEnumConverter;
    internal readonly StreamConverter _streamConverter;
    private readonly JsonNodeConverter _jsonNodeConverter;
    private readonly SnowflakeConverter _snowflakeConverter;
    private readonly ISynchronizedDictionary<Type, JsonConverter> _snowflakeDictionaryConverters;
    private readonly ISynchronizedDictionary<Type, OptionalConverter> _optionalConverters;

    public ContractResolver(DefaultJsonSerializer serializer)
    {
        _serializer = serializer;
        _stringEnumConverter = new StringEnumConverter();
        _streamConverter = new StreamConverter(serializer);
        _jsonNodeConverter = new JsonNodeConverter();
        _snowflakeConverter = new SnowflakeConverter();
        _snowflakeDictionaryConverters = new SynchronizedDictionary<Type, JsonConverter>();
        _optionalConverters = new SynchronizedDictionary<Type, OptionalConverter>();
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var jsonProperty = base.CreateProperty(member, memberSerialization);
        var jsonIgnoreAttribute = member.GetCustomAttribute<JsonIgnoreAttribute>();
        if (jsonIgnoreAttribute != null)
        {
            jsonProperty.Ignored = true;
            return jsonProperty;
        }

        var jsonPropertyAttribute = member.GetCustomAttribute<JsonPropertyAttribute>();
        if (jsonPropertyAttribute == null)
        {
            jsonProperty.Ignored = true;
            return jsonProperty;
        }

        jsonProperty.PropertyName = jsonPropertyAttribute.Name;
        jsonProperty.NullValueHandling = (Newtonsoft.Json.NullValueHandling?) jsonPropertyAttribute.NullValueHandling;

        if (jsonProperty.PropertyType!.IsGenericType && typeof(IOptional).IsAssignableFrom(jsonProperty.PropertyType))
        {
            jsonProperty.ShouldSerialize = instance => ((IOptional?) jsonProperty.ValueProvider!.GetValue(instance))!.HasValue;
            jsonProperty.Converter = GetOptionalConverter(jsonProperty.PropertyType);
        }
        else
        {
            jsonProperty.Converter = GetConverter(jsonProperty.PropertyType) ?? jsonProperty.Converter;
        }

        return jsonProperty;
    }

    protected override JsonContract CreateContract(Type objectType)
    {
        var contract = typeof(JsonModel).IsAssignableFrom(objectType)
            ? CreateObjectContract(objectType)
            : base.CreateContract(objectType);

        contract.Converter = GetConverter(objectType) ?? contract.Converter;
        return contract;
    }

    protected override JsonObjectContract CreateObjectContract(Type objectType)
    {
        var contract = base.CreateObjectContract(objectType);
        if (typeof(JsonModel).IsAssignableFrom(objectType))
        {
            contract.ExtensionDataGetter = instance =>
            {
                if (instance is not JsonModel jsonModel || !JsonModel.TryGetExtensionData(jsonModel, out var extensionData))
                    return null;

                return extensionData.Select(x =>
                {
                    var value = x.Value switch
                    {
                        null => null!,
                        DefaultJsonNode node => node.Token as object,
                        _ => JToken.FromObject(x.Value, _serializer.UnderlyingSerializer)
                    };

                    return KeyValuePair.Create(x.Key as object, value);
                });
            };

            var skippedProperties = objectType.GetCustomAttributes<JsonSkippedPropertiesAttribute>().SelectMany(x => x.Properties).Distinct().ToArray();
            contract.ExtensionDataSetter = (instance, key, value) =>
            {
                if (instance is not JsonModel model)
                    return;

                if (Array.IndexOf(skippedProperties, key) != -1)
                    return;

                var node = value switch
                {
                    null => null,
                    JToken jToken => DefaultJsonNode.Create(jToken, _serializer.UnderlyingSerializer),
                    _ => DefaultJsonNode.Create(JToken.FromObject(value, _serializer.UnderlyingSerializer), _serializer.UnderlyingSerializer)
                };

                model.ExtensionData.Add(key, node);
            };
        }

        return contract;
    }

    protected override JsonArrayContract CreateArrayContract(Type objectType)
    {
        var contract = base.CreateArrayContract(objectType);
        var elementType = objectType.GetElementType();
        if (elementType == typeof(Snowflake))
            contract.ItemConverter = _snowflakeConverter;

        return contract;
    }

    private OptionalConverter GetOptionalConverter(Type type)
    {
        var optionalType = type.GenericTypeArguments[0];
        return _optionalConverters.GetOrAdd(optionalType, (x, @this) => OptionalConverter.Create(@this.GetConverter(x)), this);
    }

    private JsonConverter? GetConverter(Type type)
    {
        if (typeof(Stream).IsAssignableFrom(type))
            return _streamConverter;

        if (typeof(IJsonNode).IsAssignableFrom(type) && !typeof(JsonModel).IsAssignableFrom(type))
            return _jsonNodeConverter;

        if (!type.IsClass)
        {
            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
                type = nullableType;

            if (type.IsEnum)
            {
                var stringEnumAttribute = type.GetCustomAttribute<StringEnumAttribute>();
                if (stringEnumAttribute != null)
                    return _stringEnumConverter;
            }
            else if (type == typeof(Snowflake))
            {
                return _snowflakeConverter;
            }
        }
        else
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var generics = type.GetGenericArguments();
                if (generics[0] == typeof(Snowflake))
                    return _snowflakeDictionaryConverters.GetOrAdd(generics[1], type => (Activator.CreateInstance(typeof(SnowflakeDictionaryConverter<>).MakeGenericType(type)) as JsonConverter)!);
            }
        }

        return null;
    }
}
