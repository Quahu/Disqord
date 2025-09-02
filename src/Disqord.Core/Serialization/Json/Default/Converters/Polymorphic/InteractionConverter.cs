using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Disqord.Models;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class InteractionConverter : PolymorphicJsonConverter<InteractionJsonModel>
{
    public override InteractionJsonModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);
        if (node == null)
        {
            Throw.InvalidOperationException("Invalid interaction node.");
        }

        var typeNode = node["type"];
        if (typeNode == null)
        {
            Throw.InvalidOperationException("Missing interaction type.");
        }

        var type = typeNode.Deserialize<InteractionType>(options);
        var interaction = node.Deserialize<InteractionJsonModel>(OptionsWithoutSelf);
        Debug.Assert(interaction != null);

        var dataType = GetInteractionDataJsonModelType(type);
        InteractionDataJsonModel? data = null;
        if (dataType != null)
        {
            var dataToken = node["data"];
            if (dataToken == null)
            {
                Throw.InvalidOperationException("Missing interaction data.");
            }

            data = (InteractionDataJsonModel?) dataToken.Deserialize(dataType, options);
            Debug.Assert(data != null);
        }

        interaction.Data = Optional.FromNullable(data);
        return interaction;
    }

    private static Type? GetInteractionDataJsonModelType(InteractionType componentType)
    {
        return componentType switch
        {
            InteractionType.Ping => null,
            InteractionType.ApplicationCommand => typeof(ApplicationCommandInteractionDataJsonModel),
            InteractionType.MessageComponent => typeof(MessageComponentInteractionDataJsonModel),
            InteractionType.ApplicationCommandAutoComplete => typeof(ApplicationCommandInteractionDataJsonModel),
            InteractionType.ModalSubmit => typeof(ModalSubmitInteractionDataJsonModel),
            _ => typeof(InteractionDataJsonModel)
        };
    }

    public override void Write(Utf8JsonWriter writer, InteractionJsonModel value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, OptionsWithoutSelf);
    }
}
