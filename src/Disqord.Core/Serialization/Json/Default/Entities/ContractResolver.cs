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
        private readonly JsonTokenConverter _jsonTokenConverter;
        private readonly SnowflakeConverter _snowflakeConverter;
        private readonly ISynchronizedDictionary<Type, OptionalConverter> _optionalConverters;

        public ContractResolver(DefaultJsonSerializer serializer)
        {
            _serializer = serializer;
            _stringEnumConverter = new StringEnumConverter();
            _streamConverter = new StreamConverter(serializer);
            _jsonTokenConverter = new JsonTokenConverter();
            _snowflakeConverter = new SnowflakeConverter();
            _optionalConverters = new SynchronizedDictionary<Type, OptionalConverter>();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            var property = new JsonModelProperty(member);
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
                jsonProperty.Converter = GetConverter(property.Type) ?? jsonProperty.Converter;
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
                    return instance is JsonModel model && model.ExtensionData != null
                        ? model.ExtensionData.Select(x => KeyValuePair.Create(x.Key as object, (x.Value as DefaultJsonToken).Token as object))
                        : null;
                };

                var skippedProperties = objectType.GetCustomAttribute<JsonSkippedPropertiesAttribute>()?.Properties;
                contract.ExtensionDataSetter = (instance, key, value) =>
                {
                    if (instance is not JsonModel model)
                        return;

                    if (skippedProperties != null && Array.IndexOf(skippedProperties, key) != -1)
                        return;

                    IJsonToken token;
                    if (value == null)
                    {
                        token = null;
                    }
                    else if (value is JToken jToken)
                    {
                        token = DefaultJsonToken.Create(jToken, _serializer.UnderlyingSerializer);
                    }
                    else
                    {
                        token = DefaultJsonToken.Create(JToken.FromObject(value, _serializer.UnderlyingSerializer), _serializer.UnderlyingSerializer);
                    }

                    model.ExtensionData.Add(key, token);
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

        private OptionalConverter GetOptionalConverter(JsonModelProperty property)
        {
            var optionalType = property.Type.GenericTypeArguments[0];
            return _optionalConverters.GetOrAdd(optionalType, (x, @this) => OptionalConverter.Create(@this.GetConverter(x)), this);
        }

        private JsonConverter GetConverter(Type type)
        {
            if (typeof(Stream).IsAssignableFrom(type))
            {
                return _streamConverter;
            }
            else if (typeof(IJsonToken).IsAssignableFrom(type) && !typeof(JsonModel).IsAssignableFrom(type))
            {
                return _jsonTokenConverter;
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

        private sealed class JsonModelProperty
        {
            public Type Type
            {
                get
                {
                    if (_member is PropertyInfo propertyInfo)
                        return propertyInfo.PropertyType;

                    return (_member as FieldInfo).FieldType;
                }
            }

            private readonly MemberInfo _member;

            public JsonModelProperty(MemberInfo member)
            {
                _member = member;
            }

            public object GetValue(object instance)
            {
                if (_member is PropertyInfo propertyInfo)
                    return propertyInfo.GetValue(instance);

                return (_member as FieldInfo).GetValue(instance);
            }

            public void SetValue(object instance, object value)
            {
                if (_member is PropertyInfo propertyInfo)
                    propertyInfo.SetValue(instance, value);

                (_member as FieldInfo).SetValue(instance, value);
            }
        }
    }
}
