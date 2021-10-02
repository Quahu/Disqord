using System.IO;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateWebhookJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("avatar")]
        public Optional<Stream> Avatar;

        public CreateWebhookJsonRestRequestContent(string name)
        {
            Name = name;
        }
    }
}
