using System;
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
            Throw.InvalidOperationException("Invalid unfurled media item node.");
        }

        var unfurledMediaItemType = node["loading_state"] != null
            ? typeof(ResolvedUnfurledMediaItemJsonModel)
            : typeof(UnfurledMediaItemJsonModel);

        var deserializeOptions = unfurledMediaItemType == typeof(UnfurledMediaItemJsonModel) ? OptionsWithoutSelf : options;
        return (UnfurledMediaItemJsonModel?) node.Deserialize(unfurledMediaItemType, deserializeOptions);
    }

    public override void Write(Utf8JsonWriter writer, UnfurledMediaItemJsonModel value, JsonSerializerOptions options)
    {
        WritePolymorphic(writer, value, options);
    }
}
