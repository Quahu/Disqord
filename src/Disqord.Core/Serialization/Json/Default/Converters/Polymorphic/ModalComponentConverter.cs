using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Disqord.Models;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class ModalComponentConverter : PolymorphicJsonConverter<ModalBaseComponentJsonModel>
{
    public override ModalBaseComponentJsonModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);
        if (node == null)
        {
            Throw.InvalidOperationException("Invalid component node.");
        }

        var type = node?["type"];
        if (type == null)
        {
            Throw.InvalidOperationException("Missing component type.");
        }

        var componentType = GetComponentJsonModelType(type.Deserialize<ComponentType>(options));
        var component = (ModalBaseComponentJsonModel?) node.Deserialize(componentType, OptionsWithPreserve);
        Debug.Assert(component != null);
        return component;
    }

    private static Type GetComponentJsonModelType(ComponentType componentType)
    {
        return componentType switch
        {
            ComponentType.Row => typeof(ModalRowComponentJsonModel),
            ComponentType.StringSelection => typeof(ModalSelectionComponentJsonModel),
            ComponentType.TextInput => typeof(ModalTextInputComponentJsonModel),
            ComponentType.Label => typeof(ModalLabelComponentJsonModel),
            _ => typeof(BaseComponentJsonModel)
        };
    }

    public override void Write(Utf8JsonWriter writer, ModalBaseComponentJsonModel value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, typeof(object), GetPolymorphicOptions(value, options));
    }
}
