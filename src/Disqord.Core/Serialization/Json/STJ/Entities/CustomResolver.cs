using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Qommon.Serialization;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Disqord.Serialization.Json.STJ.Entities;

internal class CustomResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var info = base.GetTypeInfo(type, options);

        // placeholder, replace this spaghetti with a call to jsonPropertyInfo.AttributeProvider.GetCustomAttribute in the next .NET 7 preview
        var jsonPropertyAttrs = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(prop => (PropertyAttribute: prop.GetCustomAttribute<JsonPropertyAttribute>(), IgnoreAttribute: prop.GetCustomAttribute<JsonIgnoreAttribute>(),
                JsonPropertyInfo: info.Properties.SingleOrDefault(x => x.Name == prop.Name)!))
            .Where(x=> x.JsonPropertyInfo != null);


        foreach ((JsonPropertyAttribute? attribute, JsonIgnoreAttribute? ignoreAttribute, JsonPropertyInfo jsonPropertyInfo) in jsonPropertyAttrs)
        {
            if (attribute is null || ignoreAttribute is not null)
            {
                jsonPropertyInfo.ShouldSerialize = (_, _) => false;
            }
            else
            {
                jsonPropertyInfo.Name = attribute.Name;
                jsonPropertyInfo.Options.DefaultIgnoreCondition = attribute.NullValueHandling == NullValueHandling.Ignore
                    ? JsonIgnoreCondition.WhenWritingNull
                    : JsonIgnoreCondition.Never;
            }

        }

        return info;
    }
}