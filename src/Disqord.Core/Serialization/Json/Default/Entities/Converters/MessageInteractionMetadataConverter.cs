using System;
using Disqord.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class MessageInteractionMetadataConverter : JsonConverter
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
            Throw.InvalidOperationException("Missing interaction type.");
        }

        var component = CreateMetadataJsonModel(type.ToObject<InteractionType>(serializer));
        serializer.Populate(token.CreateReader(), component);
        return component;
    }

    private static MessageInteractionMetadataJsonModel CreateMetadataJsonModel(InteractionType interactionType)
    {
        return interactionType switch
        {
            InteractionType.ApplicationCommand => new MessageApplicationCommandInteractionMetadataJsonModel(),
            InteractionType.MessageComponent => new MessageComponentInteractionMetadataJsonModel(),
            InteractionType.ModalSubmit => new MessageModalSubmitInteractionMetadataJsonModel(),
            _ => new MessageInteractionMetadataJsonModel()
        };
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
