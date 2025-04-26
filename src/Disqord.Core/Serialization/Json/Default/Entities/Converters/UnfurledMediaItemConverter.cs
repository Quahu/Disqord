using System;
using Disqord.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class UnfurledMediaItemConverter : JsonConverter
{
    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var token = JToken.ReadFrom(reader);
        var unfurledMediaItem = token["loading_state"] != null
            ? new ResolvedUnfurledMediaItemJsonModel()
            : new UnfurledMediaItemJsonModel();

        serializer.Populate(token.CreateReader(), unfurledMediaItem);
        return unfurledMediaItem;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
