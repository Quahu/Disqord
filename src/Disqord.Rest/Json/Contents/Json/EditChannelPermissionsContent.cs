using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class EditChannelPermissionsContent : JsonRequestContent
    {
        [JsonProperty("allow")]
        public ulong Allow { get; set; }

        [JsonProperty("deny")]
        public ulong Deny { get; set; }

        [JsonProperty("type")]
        public OverwriteTargetType Type { get; set; }
    }
}
