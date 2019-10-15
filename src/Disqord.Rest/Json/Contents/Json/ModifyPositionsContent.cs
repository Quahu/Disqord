using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class ModifyPositionsContent
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }
    }
}
