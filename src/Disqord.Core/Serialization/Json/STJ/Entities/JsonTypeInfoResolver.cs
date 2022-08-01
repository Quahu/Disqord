using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Qommon.Serialization;

namespace Disqord.Serialization.Json.System;

internal class JsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    private static readonly Dictionary<Type, JsonConverter> _optionalConverters = new();

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);
        var jsonProperties = jsonTypeInfo.Properties;
        var jsonPropertyCount = jsonProperties.Count;
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
        List<JsonPropertyInfo>? jsonPropertiesToRemove = null;
        for (var i = 0; i < jsonPropertyCount; i++)
        {
            var jsonProperty = jsonProperties[i];
            FieldInfo? matchingField = null;
            foreach (var property in fields)
            {
                if (jsonProperty.Name == property.Name)
                {
                    matchingField = property;
                    break;
                }
            }

            if (matchingField == null)
            {
                // TODO: IsExtensionData
                (jsonPropertiesToRemove ??= new()).Add(jsonProperty);

                continue;
            }

            // Guard.IsNotNull(matchingField);

            var attributes = matchingField.GetCustomAttributes();
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
            typeof(JsonPropertyInfo).GetProperty("Options")!.SetValue(jsonProperty, new JsonSerializerOptions(jsonProperty.Options));
            if (typeof(IOptional).IsAssignableFrom(jsonProperty.PropertyType))
            {
                jsonProperty.Options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

                ref var optionalConverter = ref CollectionsMarshal.GetValueRefOrAddDefault(_optionalConverters, jsonProperty.PropertyType, out var exists);
                if (!exists)
                {
                    var optionalConverterType = typeof(OptionalConverter<>).MakeGenericType(jsonProperty.PropertyType.GenericTypeArguments[0]);
                    optionalConverter = Unsafe.As<JsonConverter>(Activator.CreateInstance(optionalConverterType));
                }

                jsonProperty.CustomConverter = optionalConverter;
            }
            else
            {
                jsonProperty.Options.DefaultIgnoreCondition = jsonPropertyAttribute.NullValueHandling == NullValueHandling.Ignore
                    ? JsonIgnoreCondition.WhenWritingNull
                    : JsonIgnoreCondition.Never;
            }
        }

        if (jsonPropertiesToRemove != null)
        {
            foreach (var jsonProperty in jsonPropertiesToRemove)
                jsonTypeInfo.Properties.Remove(jsonProperty);
        }

        return jsonTypeInfo;
    }
}
