using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Disqord.Models;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class ComponentConverter : PolymorphicJsonConverter<BaseComponentJsonModel>
{
    public override BaseComponentJsonModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
        var component = (BaseComponentJsonModel?) node.Deserialize(componentType, OptionsWithoutSelf);
        Debug.Assert(component != null);
        return component;
    }

    private static Type GetComponentJsonModelType(ComponentType componentType)
    {
        return componentType switch
        {
            ComponentType.Row or ComponentType.Button or ComponentType.StringSelection or ComponentType.TextInput
                or ComponentType.UserSelection or ComponentType.RoleSelection
                or ComponentType.MentionableSelection or ComponentType.ChannelSelection
                => typeof(ComponentJsonModel),
            ComponentType.Section => typeof(SectionComponentJsonModel),
            ComponentType.TextDisplay => typeof(TextDisplayComponentJsonModel),
            ComponentType.Thumbnail => typeof(ThumbnailComponentJsonModel),
            ComponentType.MediaGallery => typeof(MediaGalleryComponentJsonModel),
            ComponentType.File => typeof(FileComponentJsonModel),
            ComponentType.Separator => typeof(SeparatorComponentJsonModel),
            ComponentType.Container => typeof(ContainerComponentJsonModel),
            _ => typeof(BaseComponentJsonModel)
        };
    }

    public override void Write(Utf8JsonWriter writer, BaseComponentJsonModel value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, typeof(object), GetPolymorphicOptions(value, options));
    }
}
