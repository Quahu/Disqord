using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class GuildFolderModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public ulong? Id { get; set; }

        [JsonProperty("guild_ids")]
        public ulong[] GuildIds { get; set; }

        [JsonProperty("color")]
        public int? Color { get; set; }
    }
}
