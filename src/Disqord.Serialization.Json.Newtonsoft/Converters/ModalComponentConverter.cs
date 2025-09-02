using System;
using Disqord.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qommon;

namespace Disqord.Serialization.Json.Newtonsoft;

internal sealed class ModalComponentConverter : JsonConverter
{
    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var token = JToken.ReadFrom(reader);
        var type = token["type"];
        if (type == null)
        {
            Throw.InvalidOperationException("Missing component type.");
        }

        var component = CreateComponentJsonModel(type.ToObject<ComponentType>(serializer));
        serializer.Populate(token.CreateReader(), component);
        return component;
    }

    private static ModalBaseComponentJsonModel CreateComponentJsonModel(ComponentType componentType)
    {
        return componentType switch
        {
            ComponentType.Row => new ModalRowComponentJsonModel(),
            ComponentType.StringSelection => new ModalSelectionComponentJsonModel(),
            ComponentType.TextInput => new ModalTextInputComponentJsonModel(),
            ComponentType.Label => new ModalLabelComponentJsonModel(),
            _ => new ModalBaseComponentJsonModel()
        };
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
