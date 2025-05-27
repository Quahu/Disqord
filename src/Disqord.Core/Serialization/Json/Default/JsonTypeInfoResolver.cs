using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Qommon;
using Qommon.Serialization;

namespace Disqord.Serialization.Json.Default;

internal class JsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
    private static readonly ConditionalWeakTable<JsonModel, Dictionary<string, object?>> _extensionDataCache = new();

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
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

                jsonProperty.SetIgnoreCondition(JsonIgnoreCondition.WhenWritingDefault);
            }
            else
            {
                if (jsonPropertyAttribute.NullValueHandling == NullValueHandling.Ignore)
                {
                    jsonProperty.SetIgnoreCondition(JsonIgnoreCondition.WhenWritingNull);
                }
            }
        }

        if (jsonPropertiesToRemove != null)
        {
            foreach (var jsonProperty in jsonPropertiesToRemove)
                jsonTypeInfo.Properties.Remove(jsonProperty);
        }

        if (type.IsAssignableTo(typeof(JsonModel)))
        {
            var extensionData = jsonTypeInfo.CreateJsonPropertyInfo(typeof(Dictionary<string, object?>), "InternalExtensionData");
            extensionData.IsExtensionData = true;

            // Necessary for STJ to deserialize the extension data.
            extensionData.Set = static (_, _) => { };

            extensionData.Get = obj =>
            {
                var model = Guard.IsAssignableToType<JsonModel>(obj);
                return _extensionDataCache.GetValue(model, model =>
                {
                    var extensionData = new Dictionary<string, object?>();
                    foreach (var property in model.ExtensionData)
                    {
                        extensionData[property.Key] = property.Value is JsonModel modelValue
                            ? JsonSerializer.SerializeToNode(modelValue, options)
                            : property.Value?.ToType<JsonNode>();
                    }

                    return extensionData;
                });
            };

            // Flush InternalExtensionData to JsonModel.ExtensionData
            jsonTypeInfo.OnDeserialized += obj =>
            {
                var model = Guard.IsAssignableToType<JsonModel>(obj);
                if (_extensionDataCache.TryGetValue(model, out var extensionData))
                {
                    model.ExtensionData.Clear();

                    foreach (var property in extensionData)
                    {
                        model.ExtensionData[property.Key] = DefaultJsonNode.Create(JsonSerializer.SerializeToNode(property.Value, options), options);
                    }

                    _extensionDataCache.Remove(model);
                }
            };

            jsonTypeInfo.Properties.Add(extensionData);
        }

        return jsonTypeInfo;
    }
}
