using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateInviteContent : JsonRequestContent
    {
        [JsonProperty("max_age")]
        public int MaxAge { get; set; }

        [JsonProperty("max_uses")]
        public int MaxUses { get; set; }

        [JsonProperty("temporary")]
        public bool Temporary { get; set; }

        [JsonProperty("unique")]
        public bool Unique { get; set; }
    }
}
