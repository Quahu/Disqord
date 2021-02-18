using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyGuildEmojiJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("roles")]
        public Optional<Snowflake[]> Roles;
    }
}
