using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyMessageJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("content")]
        public Optional<string> Content;

        [JsonProperty("embed")]
        public Optional<EmbedJsonModel> Embed;

        [JsonProperty("flags")]
        public Optional<MessageFlag> Flags;

        [JsonProperty("components")]
        public Optional<ComponentJsonModel[]> Components;
    }
}
