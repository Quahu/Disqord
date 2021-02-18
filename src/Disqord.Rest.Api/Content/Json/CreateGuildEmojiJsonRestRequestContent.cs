using System.IO;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateGuildEmojiJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("image")]
        public Stream Image;

        [JsonProperty("roles")]
        public Optional<Snowflake[]> Roles;
    }
}
