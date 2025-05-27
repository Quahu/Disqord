using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Disqord.Models;
using Qommon;

namespace Disqord.Serialization.Json.Default;

internal sealed class UnfurledMediaItemConverter : PolymorphicJsonConverter<UnfurledMediaItemJsonModel>
{
    public override UnfurledMediaItemJsonModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonSerializer.Deserialize<JsonNode>(ref reader, options);
        if (node == null)
        {
            Throw.InvalidOperationException("Invalid component node.");
        }

        var unfurledMediaItemType = node["loading_state"] != null
            ? typeof(ResolvedUnfurledMediaItemJsonModel)
            : typeof(UnfurledMediaItemJsonModel);

        var unfurledMediaItem = (UnfurledMediaItemJsonModel?) node.Deserialize(unfurledMediaItemType, OptionsWithoutSelf);
        Debug.Assert(unfurledMediaItem != null);
        return unfurledMediaItem;
    }

    public override void Write(Utf8JsonWriter writer, UnfurledMediaItemJsonModel value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, typeof(object), OptionsWithoutSelf);
    }
}
