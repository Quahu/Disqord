using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Disqord.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Disqord.Serialization.Json.Default
{
    internal sealed class ContractResolver : DefaultContractResolver
    {
        private readonly DefaultJsonSerializer _serializer;
        internal readonly StringEnumConverter _stringEnumConverter;
        private readonly StreamConverter _streamConverter;
        private readonly JsonElementConverter _jsonElementConverter;
        private readonly SnowflakeConverter _snowflakeConverter;
        private readonly LockedDictionary<Type, OptionalConverter> _optionalConverters;

        public ContractResolver(DefaultJsonSerializer serializer)
        {
            _serializer = serializer;
            _stringEnumConverter = new StringEnumConverter();
            _streamConverter = new StreamConverter(serializer);
            _jsonElementConverter = new JsonElementConverter();
            _snowflakeConverter = new SnowflakeConverter();
            _optionalConverters = new LockedDictionary<Type, OptionalConverter>();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            if (!(member is PropertyInfo property))
                throw new InvalidOperationException($"Json models must not contain fields ({member}).");

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

            if (jsonProperty.PropertyType.IsGenericType && typeof(IOptional).IsAssignableFrom(jsonProperty.PropertyType))
            {
                jsonProperty.ShouldSerialize = instance => ((IOptional) property.GetValue(instance)).HasValue;
                jsonProperty.Converter = GetOptionalConverter(property);
            }
            else
            {
                jsonProperty.Converter = GetConverter(property.PropertyType) ?? jsonProperty.Converter;
            }

            return jsonProperty;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);
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
                    return !(instance is JsonModel model) || model.ExtensionData == null
                        ? null
                        : model.ExtensionData.Select(x => KeyValuePair.Create(x.Key as object, x.Value));
                };

                contract.ExtensionDataSetter = (instance, key, value) =>
                {
                    if (!(instance is JsonModel model))
                        return;

                    if (model.ExtensionData == null)
                        model.ExtensionData = new Dictionary<string, object>();

                    model.ExtensionData.Add(key, value is JToken jToken
                        ? new DefaultJsonElement(jToken, _serializer._serializer)
                        : value);
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

        private OptionalConverter GetOptionalConverter(PropertyInfo property)
        {
            var optionalType = property.PropertyType.GenericTypeArguments[0];
            return _optionalConverters.GetOrAdd(optionalType, (x, @this) => OptionalConverter.Create(@this.GetConverter(x)), this);
        }

        private JsonConverter GetConverter(Type type)
        {
            if (typeof(Stream).IsAssignableFrom(type))
            {
                return _streamConverter;
            }
            else if (typeof(IJsonElement) == type)
            {
                return _jsonElementConverter;
            }
            else if (!type.IsClass)
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

            return null;
        }
    }
}
