using System;
using Disqord.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qommon;

namespace Disqord.Serialization.Json.Newtonsoft;

internal sealed class ComponentConverter : JsonConverter
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

    private static BaseComponentJsonModel CreateComponentJsonModel(ComponentType componentType)
    {
        return componentType switch
        {
            ComponentType.Row or ComponentType.Button or ComponentType.StringSelection
                or ComponentType.TextInput or ComponentType.UserSelection or ComponentType.RoleSelection
                or ComponentType.MentionableSelection or ComponentType.ChannelSelection => new ComponentJsonModel(),
            ComponentType.Section => new SectionComponentJsonModel(),
            ComponentType.TextDisplay => new TextDisplayComponentJsonModel(),
            ComponentType.Thumbnail => new ThumbnailComponentJsonModel(),
            ComponentType.MediaGallery => new MediaGalleryComponentJsonModel(),
            ComponentType.File => new FileComponentJsonModel(),
            ComponentType.Separator => new SeparatorComponentJsonModel(),
            ComponentType.Container => new ContainerComponentJsonModel(),
            _ => new BaseComponentJsonModel()
        };
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
