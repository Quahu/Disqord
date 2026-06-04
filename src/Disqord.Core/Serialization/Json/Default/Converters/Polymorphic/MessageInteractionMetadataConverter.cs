using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Disqord.Models;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class MessageInteractionMetadataConverter : PolymorphicJsonConverter<MessageInteractionMetadataJsonModel>
{
    public override MessageInteractionMetadataJsonModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);
        if (node == null)
        {
            Throw.InvalidOperationException("Invalid interaction metadata node.");
        }

        var type = node["type"];
        if (type == null)
        {
            Throw.InvalidOperationException("Missing interaction type.");
        }

        var metadataType = GetMetadataJsonModelType(type.Deserialize<InteractionType>(options));
        var deserializeOptions = metadataType == typeof(MessageInteractionMetadataJsonModel) ? OptionsWithoutSelf : options;
        return (MessageInteractionMetadataJsonModel?) node.Deserialize(metadataType, deserializeOptions);
    }

    private static Type GetMetadataJsonModelType(InteractionType interactionType)
    {
        return interactionType switch
        {
            InteractionType.ApplicationCommand => typeof(MessageApplicationCommandInteractionMetadataJsonModel),
            InteractionType.MessageComponent => typeof(MessageComponentInteractionMetadataJsonModel),
            InteractionType.ModalSubmit => typeof(MessageModalSubmitInteractionMetadataJsonModel),
            _ => typeof(MessageInteractionMetadataJsonModel)
        };
    }

    public override void Write(Utf8JsonWriter writer, MessageInteractionMetadataJsonModel value, JsonSerializerOptions options)
    {
        WritePolymorphic(writer, value, options);
    }
}
