using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Disqord.Collections.Synchronized;
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
        internal readonly StreamConverter _streamConverter;
        private readonly JsonNodeConverter _jsonNodeConverter;
        private readonly SnowflakeConverter _snowflakeConverter;
        private readonly ISynchronizedDictionary<Type, OptionalConverter> _optionalConverters;

        public ContractResolver(DefaultJsonSerializer serializer)
        {
            _serializer = serializer;
            _stringEnumConverter = new StringEnumConverter();
            _streamConverter = new StreamConverter(serializer);
            _jsonNodeConverter = new JsonNodeConverter();
            _snowflakeConverter = new SnowflakeConverter();
            _optionalConverters = new SynchronizedDictionary<Type, OptionalConverter>();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            var accessor = JsonAccessor.Create(member);
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
                jsonProperty.ShouldSerialize = instance => ((IOptional) accessor.GetValue(instance)).HasValue;
                jsonProperty.Converter = GetOptionalConverter(accessor);
            }
            else
            {
                jsonProperty.Converter = GetConverter(accessor.Type) ?? jsonProperty.Converter;
            }

            return jsonProperty;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract contract;
            if (typeof(JsonModel).IsAssignableFrom(objectType))
                contract = CreateObjectContract(objectType);
            else
                contract = base.CreateContract(objectType);
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
                    if (instance is not JsonModel model || model.ExtensionData == null)
                        return null;

                    return model.ExtensionData.Select(x =>
                    {
                        var value = x.Value switch
                        {
                            null => null,
                            DefaultJsonNode node => node.Token as object,
                            _ => JToken.FromObject(x.Value, _serializer.UnderlyingSerializer)
                        };

                        return KeyValuePair.Create(x.Key as object, value);
                    });
                };

                var skippedProperties = objectType.GetCustomAttribute<JsonSkippedPropertiesAttribute>()?.Properties;
                contract.ExtensionDataSetter = (instance, key, value) =>
                {
                    if (instance is not JsonModel model)
                        return;

                    if (skippedProperties != null && Array.IndexOf(skippedProperties, key) != -1)
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

        private OptionalConverter GetOptionalConverter(JsonAccessor accessor)
        {
            var optionalType = accessor.Type.GenericTypeArguments[0];
            return _optionalConverters.GetOrAdd(optionalType, (x, @this) => OptionalConverter.Create(@this.GetConverter(x)), this);
        }

        private JsonConverter GetConverter(Type type)
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

            return null;
        }

        private abstract class JsonAccessor
        {
            public abstract Type Type { get; }

            public abstract object GetValue(object instance);

            public abstract void SetValue(object instance, object value);

            public static JsonAccessor Create(MemberInfo memberInfo) => memberInfo switch
            {
                FieldInfo field => new FieldJsonAccessor(field),
                PropertyInfo property => new PropertyJsonAccessor(property),
                _ => throw new InvalidOperationException("Invalid member info accessor type.")
            };
        }

        private sealed class FieldJsonAccessor : JsonAccessor
        {
            public override Type Type => _field.FieldType;

            private readonly FieldInfo _field;

            public FieldJsonAccessor(FieldInfo field)
            {
                _field = field;
            }

            public override object GetValue(object instance)
                => _field.GetValue(instance);

            public override void SetValue(object instance, object value)
                => _field.SetValue(instance, value);
        }

        private sealed class PropertyJsonAccessor : JsonAccessor
        {
            public override Type Type => _property.PropertyType;

            private readonly PropertyInfo _property;

            public PropertyJsonAccessor(PropertyInfo property)
            {
                _property = property;
            }

            public override object GetValue(object instance)
                => _property.GetValue(instance);

            public override void SetValue(object instance, object value)
                => _property.SetValue(instance, value);
        }
    }
}
