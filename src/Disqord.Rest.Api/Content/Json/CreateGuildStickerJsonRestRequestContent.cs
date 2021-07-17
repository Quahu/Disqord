using System.IO;
using Newtonsoft.Json;

namespace Disqord.Rest.Api
{
    public class CreateGuildStickerJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("tags")]
        public string Tags;

        [JsonProperty("file")]
        public Stream File;
    }
}