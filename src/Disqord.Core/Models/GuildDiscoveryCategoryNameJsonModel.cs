using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class GuildDiscoveryCategoryNameJsonModel : JsonModel
    {
        [JsonProperty("default")]
        public string Default;

        [JsonProperty("localizations")]
        public Optional<IJsonObject> Localizations;
    }
}