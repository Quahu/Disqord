using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyWelcomeScreenJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("enabled")]
        public Optional<bool> Enabled;

        [JsonProperty("welcome_channels")]
        public Optional<WelcomeScreenJsonModel[]> WelcomeChannels;

        [JsonProperty("description")]
        public Optional<string> Description;
    }
}
