using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class ModifyWelcomeScreenJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("enabled")]
        public Optional<bool> Enabled;

        [JsonProperty("welcome_channels")]
        public Optional<WelcomeScreenChannelJsonModel[]> WelcomeChannels;

        [JsonProperty("description")]
        public Optional<string> Description;
    }
}
