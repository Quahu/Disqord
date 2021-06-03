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

        [JsonProperty("allowed_mentions")]
        public Optional<AllowedMentionsJsonModel> AllowedMentions;

        [JsonProperty("attachments")]
        public Optional<AttachmentJsonModel[]> Attachments;

        [JsonProperty("components")]
        public Optional<ComponentJsonModel[]> Components;
    }
}
