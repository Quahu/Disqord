using System.IO;
using Disqord.Serialization.Json;
using Qommon;

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

        public CreateGuildEmojiJsonRestRequestContent(string name, Stream image)
        {
            Name = name;
            Image = image;
        }
    }
}
