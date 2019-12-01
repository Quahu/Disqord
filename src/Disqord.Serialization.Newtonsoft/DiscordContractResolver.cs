using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Disqord.Serialization.Json.Newtonsoft
{
    internal sealed class DiscordContractResolver : DefaultContractResolver
    {
        public static readonly DiscordContractResolver Instance = new DiscordContractResolver();

        private static readonly StringEnumConverter _stringEnumConverter = new StringEnumConverter();

        private DiscordContractResolver()
        { }

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
            jsonProperty.NullValueHandling = (global::Newtonsoft.Json.NullValueHandling?) jsonPropertyAttribute.NullValueHandling;

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

        private static OptionalConverter GetOptionalConverter(PropertyInfo property)
        {
            var optionalType = property.PropertyType.GenericTypeArguments[0];
            var optionalTypeConverter = GetConverter(optionalType);
            return OptionalConverter.Create(optionalTypeConverter);
        }

        private static JsonConverter GetConverter(Type type)
        {
            if (typeof(Stream).IsAssignableFrom(type))
            {
                return StreamConverter.Instance;
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
            }

            return null;
        }
    }
}