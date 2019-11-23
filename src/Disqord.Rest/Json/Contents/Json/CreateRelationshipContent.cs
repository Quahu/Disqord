using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateRelationshipContent : JsonRequestContent
    {
        [JsonProperty("type", NullValueHandling.Ignore)]
        public RelationshipType? Type { get; set; }
    }
}
