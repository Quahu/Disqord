using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateThreadJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("auto_archive_duration")]
        public Optional<int> AutoArchiveDuration;

        [JsonProperty("type")] 
        public Optional<int?> Type;
    }
}