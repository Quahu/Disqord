using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class VoiceServerUpdateModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("guild_id")]
        public ulong? GuildId { get; set; }

        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }
    }
}
