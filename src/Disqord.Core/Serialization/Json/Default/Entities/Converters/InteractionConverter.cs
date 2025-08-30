using System;
using Disqord.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class InteractionConverter : JsonConverter
{
    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var token = JToken.ReadFrom(reader);
        var typeToken = token["type"];
        if (typeToken == null)
        {
            Throw.InvalidOperationException("Missing interaction type.");
        }

        var type = typeToken.ToObject<InteractionType>(serializer);
        var interaction = new InteractionJsonModel();
        serializer.Populate(token.CreateReader(), interaction);

        var data = CreateInteractionDataJsonModel(type);
        if (data != null)
        {
            var dataToken = token["data"];
            if (dataToken == null)
            {
                Throw.InvalidOperationException("Missing interaction data.");
            }

            serializer.Populate(dataToken.CreateReader(), data);
        }

        interaction.Data = Optional.FromNullable(data);
        return interaction;
    }

    private static InteractionDataJsonModel? CreateInteractionDataJsonModel(InteractionType componentType)
    {
        return componentType switch
        {
            InteractionType.Ping => null,
            InteractionType.ApplicationCommand => new ApplicationCommandInteractionDataJsonModel(),
            InteractionType.MessageComponent => new MessageComponentInteractionDataJsonModel(),
            InteractionType.ApplicationCommandAutoComplete => new ApplicationCommandInteractionDataJsonModel(),
            InteractionType.ModalSubmit => new ModalSubmitInteractionDataJsonModel(),
            _ => new InteractionDataJsonModel()
        };
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
